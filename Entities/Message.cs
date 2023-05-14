using System;
using ApartmentRentManagementSystem.Contracts;
using ApartmentRentManagementSystem.Identity;
namespace ApartmentRentManagementSystem.Entities
{
    public class Message : AuditableEntity
    {
        public string Text{get; set;}
        public bool IsRead{get; set;}
        public ICollection<UserMessage> UserMessages{get; set;} = new HashSet<UserMessage>();
    }
}