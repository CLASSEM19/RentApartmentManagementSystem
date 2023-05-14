
using ApartmentRentManagementSystem.Contracts;
namespace ApartmentRentManagementSystem.Entities
{
    public class Address : AuditableEntity
    {
        public string Description{get; set;}
        public string? City{get; set;}
        public string LGA{get; set;}
        public string State{get; set;}
        public string Country{get; set;}
        public Admin Admin{get; set;}
        public Customer Customer{get; set;}
        public Landlord Landlord{get; set;}
        public Apartment Apartment{get; set;}
    }
}