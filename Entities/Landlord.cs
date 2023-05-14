using System;
using ApartmentRentManagementSystem.Identity;
using System.Collections.Generic;
using System.Collections;
using ApartmentRentManagementSystem.Contracts;
namespace ApartmentRentManagementSystem.Entities
{
    public class Landlord : AuditableEntity
    {
        public string? Image{get; set;}
        public string FirstName{get; set;}
        public string LastName{get; set;}
        public string PhoneNumber{get; set;}
        public int AddressId{get; set;}
        public Address Address{get; set;}
        public int UserId{get; set;}
        public User User{get; set;}
        public string? BankCode{get; set;}
        public string? AccountNumber{get; set;}
        public ICollection<Apartment> Apartments{get; set;} = new HashSet<Apartment>();
    }
}