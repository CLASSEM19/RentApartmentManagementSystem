using System;
using ApartmentRentManagementSystem.Dtos;
using System.Threading.Tasks;
namespace ApartmentRentManagementSystem.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<BaseResponse> RegisterCategory(CategoryRequestModel model);

        // Task<BaseResponse> UpdateCategory(UpdateCategoryRequestModel model, int Id);

        // Task<CategoryResponseModel> GetCategoryById(int id);
        
        Task<CategoriesResponseModel> GetAllCategories();
        
        // Task<BaseResponse> ActivateCategory(int id);
        
        // Task<BaseResponse> DeActivateCategory(int id);
    }
}