using System;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ApartmentRentManagementSystem.Entities;
namespace ApartmentRentManagementSystem.Dtos
{
    public class UserRequestModel
    {
        public string Email{get; set;}
        public string Password{get; set;}
    }
    public class UserResponseModel : BaseResponse
    {
        public int Id{get; set;}
        public string UserName{get; set;}
        public string Email{get; set;}
        public string Image{get; set;}
        public ICollection<RoleDto> Roles{get; set;} = new HashSet<RoleDto>();
    }
    public class LoginResponse
    {
        public UserResponseModel Data{get; set;}
        public string Token{get; set;}
    }


    public class PaymentDueMessage : BaseResponse
    {
        public int ApartmentId{get; set;}
    }
}