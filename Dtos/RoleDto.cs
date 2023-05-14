using System;
using System.Collections;
using System.Collections.Generic;
namespace ApartmentRentManagementSystem.Dtos
{
    public class RoleDto
    {
        public string Name{get; set;}
        public string Description{get; set;}
    }
    public class RoleRequestModel
    {
        public string Name{get; set;}
        public string Description{get; set;}
    }
    public class RoleResponseodel : BaseResponse
    {
        public RoleDto Data{get; set;}
    }
    public class RolesResponseodel : BaseResponse
    {
        public ICollection<RoleDto> Data{get; set;} = new HashSet<RoleDto>();
    }
}