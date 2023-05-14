using System;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.IO;
using System.Collections.Generic;
namespace ApartmentRentManagementSystem.Dtos
{
    public class CustomerDto
    {
        public int Id{get; set;}
        public string Image{get; set;}
        public string FullName{get; set;}
        public string PhoneNumber{get; set;}
        public string Email{get; set;}
        public string AddressDescription{get; set;}
        public string LGA{get; set;}
        public string State{get; set;}
        public string Country{get; set;}
    }
    public class CustomerRequestModel
    { 
        public string? Image{get; set;}
        public string? FirstName{get; set;}
        public string? LastName{get; set;}
        public string? Password{get; set;}
        public string? Email{get; set;}
        public string? PhoneNumber{get; set;}
        public string? Country{get; set;}
        public string? State{get; set;}
        public string? LGA{get; set;}
        public string? AddressDescription{get; set;}
    }

    public class UpdateCustomerRequestModel
    {
        public string? Image{get; set;}
        public string? FirstName{get; set;}
        public string? LastName{get; set;}
        public string? Email{get; set;}
        public string? Password{get; set;}
        public string? PhoneNumber{get; set;}
        public string? Country{get; set;}
        public string? State{get; set;}
        public string? LGA{get; set;}
        public string? AddressDescription{get; set;}
    }
    public class CustomerResponseModel : BaseResponse
    {
        public CustomerDto Data{get; set;}
    }
    public class CustomersResponseModel : BaseResponse
    {
        public ICollection<CustomerDto> Data{get; set;} = new HashSet<CustomerDto>();
    }
}