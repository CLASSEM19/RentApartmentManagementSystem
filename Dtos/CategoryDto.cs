using System;
using System.Collections;
using System.Collections.Generic;
namespace ApartmentRentManagementSystem.Dtos
{
    public class CategoryDto
    {
        public int Id{get; set;}
        public string Name{get; set;}
        public string Description{get; set;}
        public decimal Rate{get; set;}
    }
    public class CategoryRequestModel
    {
        public string Name{get; set;}
        public string Description{get; set;}
        public decimal Rate{get; set;}
    }
    public class CategoryResponseodel : BaseResponse
    {
        public CategoryDto Data{get; set;}
    }
    public class CategoriesResponseModel : BaseResponse
    {
        public ICollection<CategoryDto> Data{get; set;} = new HashSet<CategoryDto>();
    }
}