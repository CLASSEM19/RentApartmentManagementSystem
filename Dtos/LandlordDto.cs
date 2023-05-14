using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ApartmentRentManagementSystem.Dtos
{
    public class LandlordDto
    {
        public int Id{get; set;}
        public string Image{get; set;}
        public string FullName{get; set;}
        public string PhoneNumber{get; set;}
        public string Email{get; set;}
        public string BankName{get; set;}
        public string AccountNumber{get; set;}
        public string Country{get; set;}
        public string State{get; set;}
        public string LGA{get; set;}
        public string AddressDescription{get; set;}
    }
    public class LandlordRequestModel
    {
        public string FirstName{get; set;}
        public string LastName{get; set;}
        public string Password{get; set;}
        public string Email{get; set;}
        public string PhoneNumber{get; set;}
        public string Country{get; set;}
        public string State{get; set;}
        public string LGA{get; set;}
        public string AddressDescription{get; set;}
    }
    public class UpdateLandlordRequestModel
    {
        public string? Image{get; set;}
        public string? FirstName{get; set;}
        public string? LastName{get; set;}
        public string? Email{get; set;}
        public string? PhoneNumber{get; set;}
        public string? Country{get; set;}
        public string? State{get; set;}
        public string? LGA{get; set;}
        public string? AddressDescription{get; set;}
        public string? BankName{get; set;}
        public string? AccountNumber{get; set;}
    }
    public class LandlordResponseModel : BaseResponse
    {
        public LandlordDto Data{get; set;}
    }
    public class LandlordsResponseModel : BaseResponse
    {
        public ICollection<LandlordDto> Data{get; set;} = new HashSet<LandlordDto>();
    }
}