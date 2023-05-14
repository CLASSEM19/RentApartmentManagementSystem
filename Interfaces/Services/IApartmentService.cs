using System;
using ApartmentRentManagementSystem.Dtos;
using System.Threading.Tasks;
namespace ApartmentRentManagementSystem.Interfaces.Services
{
    public interface IApartmentService
    {
        Task<ApartmentResponseModel> RegisterApartment(ApartmentRequestModel model);

        Task<BaseResponse> UpdateApartment(UpdateApartmentRequestModel model, int Id);

        Task<ApartmentResponseModel> GetApartmentById(int id);

        Task<BaseResponse> RentApartment(int id, int customerId);

        Task<ApartmentsResponseModel> GetRentedApartments();

        Task<ApartmentsResponseModel> GetAllApartments();

        Task<BaseResponse> UnRentApartment(int id);

        Task<ApartmentsResponseModel> GetUnRentedApartments();  

        Task<BaseResponse> ApproveApartment(int id); 

        Task<BaseResponse> AddCategoryToApartment(ApartmentCategory model);  

        Task<ApartmentsResponseModel> GetApprovedApartments();

        Task<ApartmentsResponseModel> GetUnApprovedApartments();

        Task<BaseResponse> DisApproveApartment(int id); 

        Task<ApartmentsResponseModel> GetApartmentsByCountry(string country);

        Task<ApartmentsResponseModel> GetApartmentsByState(string state);
        
        Task<ApartmentsResponseModel> GetApartmentsByLGA(string Lga);
    }
}