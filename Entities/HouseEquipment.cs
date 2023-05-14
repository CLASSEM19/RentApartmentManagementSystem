using System;
using ApartmentRentManagementSystem.Contracts;
namespace ApartmentRentManagementSystem.Entities
{
    public class HouseEquipment : AuditableEntity
    {
        public string Name{get; set;}
        public int Amount{get; set;}
        public string Description{get; set;}
        public int ApartmentId{get; set;}
        public Apartment Apartment{get; set;}
    }
}