using System;
using ApartmentRentManagementSystem.Contracts;
namespace ApartmentRentManagementSystem.Identity
{
    public class UserRole : AuditableEntity
    {
        public int UserId{get; set;}
        public User User{get; set;}
        public int RoleId{get; set;}
        public Role Role{get; set;}
    }
}