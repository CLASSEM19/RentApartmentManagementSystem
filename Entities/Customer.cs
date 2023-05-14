using System;
using ApartmentRentManagementSystem.Identity;
using System.Collections.Generic;
using ApartmentRentManagementSystem.Contracts;
using System.Collections;  
namespace ApartmentRentManagementSystem.Entities
{
    public class Customer : AuditableEntity
    {
        public string? Image{get; set;}
        public string FirstName{get; set;}
        public string LastName{get; set;}
        public string PhoneNumber{get; set;}
        public int AddressId{get; set;}
        public Address Address{get; set;}
        public int UserId{get; set;}
        public User User{get; set;}
        public bool IsVerified{get; set;}
        public ICollection<CustomerApartment> CustomerApartments{get; set;} = new HashSet<CustomerApartment>();
        public ICollection<Payment> Payments{get; set;} = new HashSet<Payment>();
    }
}