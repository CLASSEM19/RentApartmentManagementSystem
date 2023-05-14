using  ApartmentRentManagementSystem.Dtos;
namespace ApartmentRentManagementSystem.Authentication
{
    public interface IJWTAuthentication
    {
         string GenerateToken(UserResponseModel model);
    }
}