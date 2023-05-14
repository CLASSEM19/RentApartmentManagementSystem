using System;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ApartmentRentManagementSystem.Entities;
namespace ApartmentRentManagementSystem.Dtos
{
    public class PaymentDto
    {
        public int Id{get; set;}
        public string ReferrenceNumber{get; set;}
        public int ApartmentId{get; set;}
        public int CustomerId{get; set;}
        public decimal AmountPaidByCustomer{get; set;}
        public decimal AmountReceivedByLandlord{get; set;}
        public string DateOfPayment{get; set;}
    }
    public class PaymentRequestModel
    {
        public int ApartmentId{get; set;}
        public int CustomerId{get; set;}
        public decimal AmountPaid{get; set;}
    }
    public class PaymentByCustomerRequestModel
    {
        public int ApartmentId{get; set;}
        public int CustomerId{get; set;}
    }
    public class PaymentsResponseModel : BaseResponse
    {
       public IList<PaymentDto> Data{get; set;} =  new List<PaymentDto>();
    }
    public class SendMoneyDto
    {
        public string AccountNumber{get; set;}
        public string BankCode{get; set;} 
        public decimal Amount{get; set;}
    }
}