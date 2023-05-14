using System;
using ApartmentRentManagementSystem.Email;
using ApartmentRentManagementSystem.Dtos;
using ApartmentRentManagementSystem.Entities;
using ApartmentRentManagementSystem.Interfaces.Repositories;
using ApartmentRentManagementSystem.Interfaces.Services;

namespace ApartmentRentManagementSystem.Implementations.Services
{
    public class PaymentDueService : IPaymentDueService
    {
        private readonly IEmailSender _email;

        private readonly ICustomerRepository _customerRepository;

        private readonly IApartmentRepository _apartmentRepository;

        public PaymentDueService(ICustomerRepository customerRepository, IApartmentRepository apartmentRepository)
        {
            _apartmentRepository = apartmentRepository;
            _customerRepository = customerRepository;
        }


     
       public async Task<bool> SendPaymentDueMessage()
       {
            var customers = await _customerRepository.GetAllCustomerWithUser();
            if (customers == null)
            {
                return false;
            }
            var apartments = await _apartmentRepository.GetAllApartments();
            foreach (var item in customers)
            {
                var myapartment = apartments.Where(a => a.RentBy == item.Id);
                if (myapartment != null)
                {
                    foreach (var apartment in myapartment)
                    {
                        var stringexpDate = apartment.PaymentExpiryDate.ToString().Split(" ")[0];
                        var expDate = DateTime.Parse(stringexpDate);
                        var stringNowDate = apartment.PaymentExpiryDate.ToString().Split(" ")[0];
                        var nowDate = DateTime.Parse(stringNowDate);
                        var expMonths = (expDate.Month+ (expDate.Year * 12));
                        var nowMonths = (nowDate.Month+ (nowDate.Year * 12));
                            if (expDate == nowDate)
                            {
                                apartment.IsRented = false;
                                    apartment.RentBy = 0;
                                    await _apartmentRepository.Update(apartment);
                                    
                                     var emailRequest = new EmailRequestModel{
                                    ReceiverName = item.User.UserName,
                                    ReceiverEmail = item.User.Email,
                                    Message = "Your payment has finally due for this Apartment",
                                    Subject = "Payment Due"
                                };
                                await _email.SendEmail(emailRequest);

                                     var emailReq = new EmailRequestModel{
                                    ReceiverName = "Admin",
                                    ReceiverEmail = "ajibikeambaaq@gmail.com",
                                    Message = $"{item.User.UserName} apartment as due",
                                    Subject = "Payment Due"
                                };
                                await _email.SendEmail(emailReq);
                            }
                            else if ((expMonths - nowMonths)  <= 3)
                            {
                                  var emailRequest = new EmailRequestModel{
                                    ReceiverName = item.User.UserName,
                                    ReceiverEmail = item.User.Email,
                                    Message = $"The payment for this apartment will due in the next {(expMonths - nowMonths)} months",
                                    Subject = "Payment Due"
                                };
                                await _email.SendEmail(emailRequest);

                                     var emailReq = new EmailRequestModel{
                                    ReceiverName = "Admin",
                                    ReceiverEmail = "ajibikeambaaq@gmail.com",
                                    Message = $"{item.User.UserName} apartment is about to  due",
                                    Subject = "Payment Due"
                                };
                                await _email.SendEmail(emailReq);
                            }
                    }
                }
            }
            return true;
           
       }
       
    }
}