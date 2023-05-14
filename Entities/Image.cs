using ApartmentRentManagementSystem.Contracts;
namespace ApartmentRentManagementSystem.Entities
{
    public class Image : AuditableEntity
    {
        public string? Path{get; set;}
        public int ApartmentId{get; set;}
        public Apartment Apartment{get; set;}
    }
}