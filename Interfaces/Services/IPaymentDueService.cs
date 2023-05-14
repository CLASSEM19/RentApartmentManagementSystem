using System;
using ApartmentRentManagementSystem.Dtos;
using System.Threading.Tasks;
namespace ApartmentRentManagementSystem.Interfaces.Services
{
    public interface IPaymentDueService
    {
        Task<bool> SendPaymentDueMessage();
    }
}