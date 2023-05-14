using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ApartmentRentManagementSystem.Dtos
{
    public class AdminDto
    {
        public int Id{get; set;}
        public string Image{get; set;}
        public string FullName{get; set;}
        public string PhoneNumber{get; set;}
        public string Email{get; set;}
        public string Address{get; set;}
    }
    public class AdminRequestModel
    {
        public IFormFile Image{get; set;}
        public string FirstName{get; set;}
        public string LastName{get; set;}
        public string Password{get; set;}
        public string Email{get; set;}
        public string PhoneNumber{get; set;}
        public string Country{get; set;}
        public string State{get; set;}
        public string LGA{get; set;}
        public string AddressDescription{get; set;}
        public string City{get; set;}
    }
    public class UpdateAdminRequestModel
    {
        public IFormFile Image{get; set;}
        public string Email{get; set;}
        public string Password{get; set;}
        public string PhoneNumber{get; set;}
        public string Country{get; set;}
        public string State{get; set;}
        public string LGA{get; set;}
        public string AddressDescription{get; set;}
        public string City{get; set;}
    }
    public class AdminResponseModel : BaseResponse
    {
        public AdminDto Data{get; set;}
    }
    public class AdminsResponseModel : BaseResponse
    {
        public ICollection<AdminDto> Data{get; set;} = new HashSet<AdminDto>();
    }
}