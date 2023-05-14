using ApartmentRentManagementSystem.Entities;
using ApartmentRentManagementSystem.Dtos;
namespace ApartmentRentManagementSystem.Email
{
    public interface IEmailSender
    {
        Task<bool> SendEmail(EmailRequestModel email);
    }
}