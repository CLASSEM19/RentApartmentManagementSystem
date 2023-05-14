using System;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ApartmentRentManagementSystem.Entities;
namespace ApartmentRentManagementSystem.Dtos
{
    public class ApartmentDto
    {
        public bool? IsRented{get; set;}
        public bool? IsApproved{get; set;}
        public int? Id{get; set;}
        public string? Address{get; set;}
        public string? Period{get; set;}
        public string? TermsAndCondition{get; set;}
        public string? ApartmentNumber{get; set;}
        public decimal? Price{get; set;}
        public decimal? ApartmentTotalPrice{get; set;}
        public int? LandlordId{get; set;}
        public string Country{get; set;}
        public string State{get; set;}
        public string LGA{get; set;}
        public string AddressDescription{get; set;}
        public ICollection<ImageDto> Images{get; set;} = new HashSet<ImageDto>();
        public string? Category{get; set;}
        public ICollection<HouseEquipmentDto> HouseEquipments{get; set;} = new HashSet<HouseEquipmentDto>();
        public ICollection<UtilityDto> Utilities{get; set;} = new HashSet<UtilityDto>();
    }
    public class ApartmentRequestModel
    {
        public string Country{get; set;}
        public string State{get; set;}
        public string LGA{get; set;}
        public string AddressDescription{get; set;}
        public IFormFile TermsAndCondition{get; set;}
        public decimal Price{get; set;}
        public decimal DamagesFee{get; set;}
        public int LandlordId{get; set;}
        public ICollection<string> Images{get; set;} = new HashSet<string>();
    }
    
    public class UpdateApartmentRequestModel
    {
        public IFormFile? TermsAndCondition{get; set;}
        public decimal? Price{get; set;}
        public ICollection<string> Images{get; set;} = new HashSet<string>();
    }
    
    public class ApartmentCategory
    {
        public int ApartmentId{get; set;}
        public string Category{get; set;}
    }
    public class ApartmentResponseModel : BaseResponse
    {
        public ApartmentDto Data{get; set;}
    }
    public class ApartmentsResponseModel : BaseResponse
    {
        public ICollection<ApartmentDto> Data{get; set;} = new HashSet<ApartmentDto>();
    }
    public class SearchRequest
    {
        public int NumberOfRooms{get; set;}
        public string LGA{get; set;}
    }
}