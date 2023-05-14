using ApartmentRentManagementSystem.Entities;
using ApartmentRentManagementSystem.Dtos;
namespace ApartmentRentManagementSystem.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserResponseModel> Login(UserRequestModel model);
    }
}