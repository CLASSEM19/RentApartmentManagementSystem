using System;
using ApartmentRentManagementSystem.Contracts;
namespace ApartmentRentManagementSystem.Entities
{
    public class Payment : AuditableEntity
    {
        public string ReferrenceNumber{get; set;}
        public int CustomerId{get; set;}
        public Customer Customer{get; set;}
        public decimal AmountPaid{get; set;}
        public string DateOfPayment{get; set;}
        public decimal AmountSendToLandlord{get; set;}
        public int ApartmentId{get; set;}
        public Apartment Apartment{get; set;}
        public bool IsVerified{get; set;}
    }
}