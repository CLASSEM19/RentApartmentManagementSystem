using System;
using System.Collections.Generic;
using System.Collections;
using ApartmentRentManagementSystem.Contracts;
namespace ApartmentRentManagementSystem.Entities
{
    public class Apartment : AuditableEntity
    {
        public string ApartmentNumber{get; set;} = $"Apart {Guid.NewGuid().ToString().Substring(0, 5)}";
        public bool IsRented{get; set;}
        public IList<Image> Images{get; set;} = new List<Image>();
        public int AddressId{get; set;}
        public Address Address{get; set;}
        public int LandlordId{get; set;}
        public Landlord Landlord{get; set;}
        public string? Period{get; set;}
        public string TermsAndCondition{get; set;}
        public decimal DamagesFee{get; set;}
        public decimal DamagesBalance{get; set;}
        public DateTime PaymentExpiryDate{get; set;}
        public decimal? Price{get; set;}  
        public bool IsApproved{get; set;}
        public int RentBy{get; set;}
        public decimal? ApartmentTotalPrice{get; set;}
        public int? CategoryId{get; set;}
        public Category Category{get; set;}
        public ICollection<HouseEquipment> HouseEquipments{get; set;} = new HashSet<HouseEquipment>();
        public ICollection<Utility> Utilities{get; set;} = new HashSet<Utility>();
        public ICollection<CustomerApartment> CustomerApartments{get; set;} = new HashSet<CustomerApartment>();
        public ICollection<Complaint> Complaints{get; set;} = new HashSet<Complaint>();
        public ICollection<Payment> Payments{get; set;} = new HashSet<Payment>();
    }
}