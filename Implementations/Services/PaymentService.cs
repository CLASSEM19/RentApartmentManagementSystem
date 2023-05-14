using System;
using ApartmentRentManagementSystem.Dtos;
using ApartmentRentManagementSystem.Entities;
using System.Threading.Tasks;
using System.IO;
using ApartmentRentManagementSystem.Interfaces.Services;
using ApartmentRentManagementSystem.Interfaces.Repositories;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using ApartmentRentManagementSystem.Email;



namespace ApartmentRentManagementSystem.Implementations.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IEmailSender _email;

        private readonly IPaymentRepository _paymentRepository;

        private readonly ICustomerRepository _customerRepository;

        private readonly IApartmentRepository _apartmentRepository;

        private readonly ILandlordRepository _landlordRepository;

        public PaymentService(IEmailSender email, ILandlordRepository landlordRepository, IPaymentRepository PaymentRepository, ICustomerRepository customerRepository, IApartmentRepository apartmentRepository)
        {
            _email  = email;
            _paymentRepository = PaymentRepository;
            _apartmentRepository = apartmentRepository;
            _customerRepository = customerRepository;
            _landlordRepository = landlordRepository;
        }
        public async Task<PaystackResponse> MakePayment(PaymentRequestModel model)
        {
            var apartment = await _apartmentRepository.GetApartmentInfo(model.ApartmentId);
            if (apartment == null)
            {
                return new PaystackResponse
                {
                    status = false,
                    message = "Apartment not found"
                };
            }
            var customer = await _customerRepository.GetCustomerInfo(model.CustomerId);
            if (customer == null)
            {
                return new PaystackResponse
                {
                    status = false,
                    message = "You are  not a customer"
                };
            }
            if(customer.IsVerified == false)
            {
                return new PaystackResponse
                {
                    status = false,
                    message = "You have not been verified"
                };
            }
            var  landlord = await _landlordRepository.Get(x => x.Id == apartment.LandlordId);
            
            
            if (apartment == null)
            {
                return new PaystackResponse
                {
                    status = false,
                    message = "Apartment not found"
                };
            }
            
            var pay = model.AmountPaid * apartment.Category.Rate;
          var generateRef  = Guid.NewGuid().ToString().Substring(0, 10);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var url =  "https://api.paystack.co/transaction/initialize";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sk_test_c43bd7866c1bd0dd38c7bfcea1c03290ae02d5d3");
            var content = new StringContent(JsonSerializer.Serialize(new {
                amount = model.AmountPaid * 100,
                email = customer.User.Email,
                referrenceNumber = generateRef
                
            }), Encoding.UTF8, "application/json");
            var response  = await client.PostAsync(url, content);
            var resString = await response.Content.ReadAsStringAsync();
            var responseObj = JsonSerializer.Deserialize<PaystackResponse>(resString);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                if(apartment.IsApproved == true && apartment.IsRented == false)
                {
                    apartment.IsRented = true;
                    apartment.RentBy = customer.Id;
                    await _apartmentRepository.Update(apartment);
                    return new PaystackResponse
                    {
                        status = true,
                        message = "Apartment rent and payment successfull"
                    };
                }
                int multi = 0;
                if (apartment.IsRented == true)
                {
                    var r = ((apartment.Price/model.AmountPaid) * 10).ToString();
                    var splitM = r.Split(".");
                    var m = int.Parse(splitM[0]);
                    if (m >= 10)
                    {
                        multi = 12;
                        apartment.PaymentExpiryDate = apartment.PaymentExpiryDate.AddMonths(12);
                        await _apartmentRepository.Update(apartment);
                    } 
                    else if (m >= 5 && m < 10)
                    {
                        multi = 6;
                        apartment.PaymentExpiryDate = apartment.PaymentExpiryDate.AddMonths(6);
                        await _apartmentRepository.Update(apartment);
                    }
                    else 
                    {
                        multi = 1;
                        apartment.PaymentExpiryDate = apartment.PaymentExpiryDate.AddMonths(3);
                        await _apartmentRepository.Update(apartment);
                    }              
                }
                var sendMoneyDto = new SendMoneyDto
                {
                   AccountNumber = landlord.AccountNumber,
                   BankCode = landlord.BankCode,
                   Amount = pay
                };
               var sendLandlordMoney = await VerifyAccountNumber(sendMoneyDto.AccountNumber, sendMoneyDto.BankCode, sendMoneyDto.Amount);
                
                return new PaystackResponse
                {
                    status = true,
                    message = responseObj.message,
                    data = new PaystackData{
                        authorization_url = responseObj.data.authorization_url,
                        reference = generateRef
                    }
                };
            }
           
            
                   var payment = new Payment
             {
                 ApartmentId = model.ApartmentId,
                 CustomerId = customer.Id,
                 AmountPaid = model.AmountPaid,
                 AmountSendToLandlord = pay,
                 DateOfPayment = DateTime.UtcNow.ToString(),
                 ReferrenceNumber = generateRef
                 
             };
             
             var createPayment = await _paymentRepository.Register(payment);
             if (createPayment == null)
             {
                return new PaystackResponse
                {
                    message = "Payment not successfull try again",
                    status = false,
                };
             }
                var emailRequest = new EmailRequestModel{
                ReceiverName = landlord.User.UserName,
                ReceiverEmail = landlord.User.Email,
                Message = "A customer has paid for your apartment",
                Subject = "Payment"
            };
            await _email.SendEmail(emailRequest);
                return new PaystackResponse
                {
                    message = responseObj.message,
                    status = true
                };
        }

        private async Task<MakeATransfer> SendMoney(string recip, decimal amount)
        {
            var getHttpClient = new HttpClient();
            getHttpClient.DefaultRequestHeaders.Accept.Clear();
            getHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var baseUri = $"https://api.paystack.co/transfer";
            getHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sk_test_c43bd7866c1bd0dd38c7bfcea1c03290ae02d5d3");
            var response = await getHttpClient.PostAsJsonAsync(baseUri, new
            {
                
                recipient = recip,
                amount = amount * 100,
                currency = "NGN",
                source = "balance"
            });
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<MakeATransfer>(responseString);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (!responseObject.status)
                {
                    return new MakeATransfer()
                    {
                        status = false,
                        message = responseObject.message
                    };
                }
                return new MakeATransfer()
                {
                    status = true,
                    message = responseObject.message,
                    data = responseObject.data
                };
            }
            return new MakeATransfer()
            {
                status = false,
                message = "Pls retry payment is not successfull"
            };
        }

        public async Task<GenerateRecipient> GenerateRecipients(VerifyBank verifyBank)
        {
            
            var getHttpClient = new HttpClient();
            getHttpClient.DefaultRequestHeaders.Accept.Clear();
            getHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var baseUri = getHttpClient.BaseAddress = new Uri($"https://api.paystack.co/transferrecipient");
            getHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sk_test_c43bd7866c1bd0dd38c7bfcea1c03290ae02d5d3");
            var response = await getHttpClient.PostAsJsonAsync(baseUri, new
            {
                type = "nuban",
                name = verifyBank.data.account_name,
                account_number = verifyBank.data.account_number,
                bank_code = verifyBank.data.bank_code,
                currency = "NGN",
            });
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<GenerateRecipient>(responseString);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                if (!responseObject.status)
                {
                    return new GenerateRecipient()
                    {
                        status = false,
                        message = responseObject.message
                    };
                }
                return new GenerateRecipient()
                {
                    status = true,
                    message = "Recipient Generated",
                    data = responseObject.data
                };
            }

            return new GenerateRecipient()
            {
                status = false,
                message = responseObject.message
            };
        }



        public async Task<VerifyBank> VerifyAccountNumber(string acNumber, string bankCode, decimal amount)
        {
             var getHttpClient = new HttpClient();
            getHttpClient.DefaultRequestHeaders.Accept.Clear();
            getHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var baseUri = getHttpClient.BaseAddress =
                new Uri($"https://api.paystack.co/bank/resolve?account_number={acNumber}&bank_code={bankCode}");

            getHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "sk_test_6483775b59a2152f947af8583a987e98eb5c7af2");
            var response = await getHttpClient.GetAsync(baseUri);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<VerifyBank>(responseString);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (!responseObject.status)
                {
                    return new VerifyBank()
                    {
                        status = false,
                        message = responseObject.message
                    };
                }

                var splitName = responseObject.data.account_name;
                var splitNameArray = splitName.Split(' ');
                var generate = await GenerateRecipients(responseObject);
                if (!generate.status)
                {
                    return new VerifyBank()
                    {
                        status = false,
                        message = generate.message
                    };
                }

                var makeTransfer = await SendMoney(generate.data.recipient_code, amount);
                if (!makeTransfer.status)
                {
                    return new VerifyBank()
                    {
                        status = false,
                        message = makeTransfer.message
                    };
                }
                return new VerifyBank()
                {
                    status = true,
                    message = makeTransfer.message,
                    data = new VerifyBankData()
                    {
                        reason = generate.data.reason,
                        reference = generate.data.reference,
                        recipient_code = generate.data.recipient_code,
                        amount = makeTransfer.data.amount,
                        currency = makeTransfer.data.currency,
                        status = makeTransfer.data.status,
                        transfer_code = makeTransfer.data.transfer_code
                    }
                };
            }

            return new VerifyBank()
            {
                status = false,
                message = "Cannot verify account number"
            };
            
        }













        public async Task<BaseResponse> VerifyPayment(string referrenceNumber)
        {
            var payment = await _paymentRepository.Get(x => x.ReferrenceNumber == referrenceNumber);
            if (payment == null)
            {
                return new BaseResponse{
                    Message = "Payment not found",
                    Status = false
                };
            }
            payment.IsVerified = true;
            var updatePayment = await _paymentRepository.Update(payment);
            return new BaseResponse 
            {
                Status = true,
                Message = "Verification successfull"
            };
        }


        public async Task<PaymentsResponseModel> GetAllPaymentsByCustomer(int userId)
        {
            var customer = await _customerRepository.GetCustomerInfo(userId);
            var payments = await _paymentRepository.GetByExpression(x => x.CustomerId == customer.Id);
            if(payments == null)
            {
                return new PaymentsResponseModel
                {
                    Message = "No payment made by this customer",
                    Status = false
                };
            }
            var paymentsDto = payments.Select(p => new PaymentDto
            {
                Id = p.Id,
                ApartmentId = p.ApartmentId,
                CustomerId = p.CustomerId,
                AmountPaidByCustomer = p.AmountPaid,
                AmountReceivedByLandlord = p.AmountSendToLandlord,
                ReferrenceNumber = p.ReferrenceNumber,
                DateOfPayment = p.CreatedOn.ToString()
            }).ToList();
            return new PaymentsResponseModel
            {
                Data = paymentsDto,
                Message = "All Payments by this Customer",
                Status = true
            };
        }


        public async Task<PaymentsResponseModel> GetAllApartmentPayments(int apartmentId)
        {
            var payments = await _paymentRepository.GetByExpression(x => x.ApartmentId == apartmentId);
            if(payments == null)
            {
                return new PaymentsResponseModel
                {
                    Message = "No payment made to this apartment",
                    Status = false
                };
            }

            return new PaymentsResponseModel
            {
                Data = payments.Select(p => new PaymentDto
                {
                    Id = p.Id,
                    ApartmentId = p.ApartmentId,
                    CustomerId = p.CustomerId,
                    AmountPaidByCustomer = p.AmountPaid,
                    AmountReceivedByLandlord = p.AmountSendToLandlord,
                    ReferrenceNumber = p.ReferrenceNumber,
                    DateOfPayment = p.CreatedOn.ToString()
                }).ToList(),
                Message = "All Payments by this Customer",
                Status = true
            };
        }

        public async Task<PaymentsResponseModel> GetAllApartmentPaymentsByCustomer(int apartmentId, int customerId)
        {
            var payments = await _paymentRepository.GetByExpression(x => x.ApartmentId == apartmentId && x.CustomerId == customerId);
            if(payments == null)
            {
                return new PaymentsResponseModel
                {
                    Message = "No payment made to this apartment",
                    Status = false
                };
            }

            return new PaymentsResponseModel
            {
                Data = payments.Select(p => new PaymentDto
                {
                    Id = p.Id,
                    ApartmentId = p.ApartmentId,
                    CustomerId = p.CustomerId,
                    AmountPaidByCustomer = p.AmountPaid,
                    AmountReceivedByLandlord = p.AmountSendToLandlord,
                    ReferrenceNumber = p.ReferrenceNumber,
                    DateOfPayment = p.CreatedOn.ToString()
                }).ToList(),
                Message = "All Payments by this Customer",
                Status = true
            };
        }

    }
}