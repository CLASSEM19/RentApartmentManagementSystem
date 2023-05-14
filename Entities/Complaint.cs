using System;
using ApartmentRentManagementSystem.Identity;
using System.Collections.Generic;
using ApartmentRentManagementSystem.Contracts;
using System.Collections;  
namespace ApartmentRentManagementSystem.Entities
{
    public class Complaint : AuditableEntity
    {
        public string Problem{get; set;}
        public decimal SolutionFee{get; set;}
        public bool IsSolved{get; set;}
        public int ApartmentId{get; set;}
        public Apartment Apartment{get; set;}
    }
}