using ApartmentRentManagementSystem.Contracts;
namespace ApartmentRentManagementSystem.Entities
{
    public class CustomerApartment : AuditableEntity
    {
        public int CustomerId{get; set;}
        public Customer Customer{get; set;}
        public int ApartmentId{get; set;}
        public Apartment Apartment{get; set;}
    }
}