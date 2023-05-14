using ApartmentRentManagementSystem.Entities;
using ApartmentRentManagementSystem.Dtos;
namespace ApartmentRentManagementSystem.Interfaces.Services
{
    public interface IImageService
    {
        Task<BaseResponse> RegisterImage(string model);
    }
}