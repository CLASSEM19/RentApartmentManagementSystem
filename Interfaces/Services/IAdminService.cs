using System;
using ApartmentRentManagementSystem.Dtos;
using System.Threading.Tasks;
namespace ApartmentRentManagementSystem.Interfaces.Services
{
    public interface IAdminService
    {
        Task<BaseResponse> RegisterAdmin(AdminRequestModel model);

        Task<BaseResponse> UpdateAdmin(UpdateAdminRequestModel model, int Id);

        Task<AdminResponseModel> GetAdminInfo(int id);
        
        Task<AdminsResponseModel> GetAllAdmins();

        Task<BaseResponse> ActivateAdmin(int id);

        Task<BaseResponse> DeActivateAdmin(int id);
        
        Task<AdminsResponseModel> GetAllActivatedAdmins();
 
        Task<AdminsResponseModel> GetAllDeactivatedAdmins();
    }
}