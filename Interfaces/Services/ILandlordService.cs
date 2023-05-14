using System;
using ApartmentRentManagementSystem.Dtos;
using System.Threading.Tasks;
namespace ApartmentRentManagementSystem.Interfaces.Services
{
    public interface ILandlordService
    {
        Task<BaseResponse> RegisterLandlord(LandlordRequestModel model);

        Task<BaseResponse> UpdateLandlord(UpdateLandlordRequestModel model, int Id);

        Task<LandlordResponseModel> GetLandLordByEmail(string email);

        Task<LandlordResponseModel> GetLandLordById(int id);
        
        Task<LandlordsResponseModel> GetAllLandlords();

        Task<ApartmentsResponseModel> GetApartmentsByUserId(int id);

        Task<BaseResponse> ActivateLandlord(int id);

        Task<LandlordResponseModel> GetLandLordInfo(int id);

        Task<BaseResponse> DeActivateLandlord(int id);

        Task<ApartmentsResponseModel> GetApartmentsByLandlord(int id);
        
        Task<LandlordsResponseModel> GetAllActivatedLandlords();
 
        Task<LandlordsResponseModel> GetAllDeactivatedLandlords();

        Task<ApartmentsResponseModel> GetApprovedApartmentsByLandlord(int id);

        Task<ApartmentsResponseModel> GetUnApprovedApartmentsByLandlord(int id);

        Task<ApartmentsResponseModel> GetUnRentedApartmentsByLandlord(int id);
        
        Task<ApartmentsResponseModel> GetRentedApartmentsByLandlord(int id);
  
    }
}