using System;
using ApartmentRentManagementSystem.Dtos;
using ApartmentRentManagementSystem.Entities;
using System.Threading.Tasks;
namespace ApartmentRentManagementSystem.Interfaces.Services
{
    public interface IComplaintService
    {
       Task<BaseResponse> MakeComplaint(ComplaintRequestModel model);

        Task<ComplaintsResponseModel> GetSolvedComplaintsByAparmentId(int apartmentId);

        Task<ComplaintsResponseModel> GetUnSolvedComplaintsByAparmentId(int apartmentId);

        Task<BaseResponse> SolveComplaint(int complaintId);

        Task<ComplaintsResponseModel> GetAllUnSolvedComplaints();

        Task<DashBoard> ShowDashBoard();
    }
}