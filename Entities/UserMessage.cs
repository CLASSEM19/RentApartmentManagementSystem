using ApartmentRentManagementSystem.Contracts;
using ApartmentRentManagementSystem.Identity;
namespace ApartmentRentManagementSystem.Entities
{
    public class UserMessage : AuditableEntity
    {
        public int UserId{get; set;}
        public User User{get; set;}
        public int MessageId{get; set;}
        public Message Message{get; set;}
    }
}