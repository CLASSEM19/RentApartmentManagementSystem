using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ApartmentRentManagementSystem.Email
{
    public class EmailRequestModel
    {
        public string ReceiverName{get; set;}
        public string ReceiverEmail{get; set;}
        public string Message{get; set;}
        public string Subject{get; set;}
    } 
    public class EmailResponseModel
    {
        public string Message{get; set;}
    } 
}