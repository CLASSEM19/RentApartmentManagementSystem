using ApartmentRentManagementSystem.Entities;
using ApartmentRentManagementSystem.Dtos;
namespace ApartmentRentManagementSystem.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<PaystackResponse> MakePayment(PaymentRequestModel model);

        Task<BaseResponse> VerifyPayment(string referrenceNumber);

        Task<PaymentsResponseModel> GetAllPaymentsByCustomer(int customerId);

        Task<PaymentsResponseModel> GetAllApartmentPayments(int apartmentId);

        Task<PaymentsResponseModel> GetAllApartmentPaymentsByCustomer(int apartmentId, int customerId);
    }
}