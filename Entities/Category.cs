using System;
using ApartmentRentManagementSystem.Identity;
using System.Collections.Generic;
using ApartmentRentManagementSystem.Contracts;
using System.Collections;  
namespace ApartmentRentManagementSystem.Entities
{
    public class Category : AuditableEntity
    {
        public string Name{get; set;}
        public string Description{get; set;}
        public decimal Rate{get; set;}
        public ICollection<Apartment> Apartments{get; set;} = new HashSet<Apartment>();
    }
}