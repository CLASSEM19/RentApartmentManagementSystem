using System;
using ApartmentRentManagementSystem.Dtos;
using System.Threading.Tasks;
namespace ApartmentRentManagementSystem.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<BaseResponse> RegisterCustomer(CustomerRequestModel model);

        Task<BaseResponse> UpdateCustomer(UpdateCustomerRequestModel model, int Id);

        Task<CustomerResponseModel> GetCustomerInfo(int id);
        
        Task<CustomersResponseModel> GetAllCustomers();

        Task<ApartmentsResponseModel> GetApartmentsByCustomerId(int id);

        Task<BaseResponse> VerifyCustomer(int id);

        Task<CustomersResponseModel> GetAllVerifiedCustomers();

        Task<CustomersResponseModel> GetNotVerifiedCustomers();
    }
}