using System;
using ApartmentRentManagementSystem.Dtos;
using ApartmentRentManagementSystem.Entities;
using ApartmentRentManagementSystem.Interfaces.Repositories;
using ApartmentRentManagementSystem.Interfaces.Services;
namespace ApartmentRentManagementSystem.Implementations.Services
{
    public class CategoryService : ICategoryService
    { 
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<BaseResponse> RegisterCategory(CategoryRequestModel model)
        {
            if (model == null)
            {
                return new BaseResponse
                {
                    Status = false,
                    Message = "Value cannot be nuull"
                };
            }
            
            var cate = new Category
            {
                Name = model.Name,
                Description = model.Description,
                Rate = model.Rate
            };
            System.Console.WriteLine($"model rate, {model.Rate}");
            await _categoryRepository.Register(cate);
            return new BaseResponse
            {
                Status = true,
                Message = "New category succussfully added"
            };
        }

        public async Task<CategoriesResponseModel> GetAllCategories()
        {
              var categories = await _categoryRepository.GetAll();
           var categoryDto = categories.Select(x => new CategoryDto
           {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Rate = x.Rate
           }).ToList();

           return new CategoriesResponseModel
           {
               Data = categoryDto,
               Message = "List of all categories",
                Status = true
           };
        }
    }
}