using System;
using ApartmentRentManagementSystem.Contracts;
using System.Collections.Generic;
using System.Collections;
using ApartmentRentManagementSystem.Entities;
namespace ApartmentRentManagementSystem.Identity
{
    public class User : AuditableEntity
    {
        public string UserName{get; set;}
        public string Password{get; set;}
        public string Email{get; set;}
        public Admin Admin{get; set;}
        public Customer Customer{get; set;}
        public Landlord Landlord{get; set;}
        public ICollection<UserRole> UserRoles{get; set;} = new HashSet<UserRole>();
        public ICollection<UserMessage> UserMessages{get; set;} = new HashSet<UserMessage>();
    }    
}