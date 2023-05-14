using System;
using ApartmentRentManagementSystem.Email;
using ApartmentRentManagementSystem.Dtos;
using ApartmentRentManagementSystem.Entities;
using ApartmentRentManagementSystem.Interfaces.Repositories;
using ApartmentRentManagementSystem.Interfaces.Services;

namespace ApartmentRentManagementSystem.Implementations.Services
{
    public class ComplaintService : IComplaintService
    {
        private readonly IEmailSender _email;

        private readonly IComplaintRepository _complaintRepository;

        private readonly ICustomerRepository _customerRepository;

        private readonly IApartmentRepository _apartmentRepository;

        public ComplaintService(IComplaintRepository complaintRepository, ICustomerRepository customerRepository, IApartmentRepository apartmentRepository)
        {
            _complaintRepository = complaintRepository;
            _apartmentRepository = apartmentRepository;
            _customerRepository = customerRepository;
        }


       public async Task<BaseResponse> MakeComplaint(ComplaintRequestModel model)
       {
           var apartment = await _apartmentRepository.Get(x => x.ApartmentNumber == model.ApartmentNumber);
           if (apartment == null)
           {
               return new BaseResponse
               {
                   Message = "Apartment not found",
                   Status = false
               };
           }       
                    var complaint = new Complaint
                    {
                        Problem =  model.ProblemDescription,
                        ApartmentId = apartment.Id
                    };
                    await _complaintRepository.Register(complaint);
           
            return new BaseResponse
            {
                Message = "complaint Successfully created",
                Status = true
            };
       }
       public async Task<ComplaintsResponseModel> GetSolvedComplaintsByAparmentId(int apartmentId)
       {
            var complaint = await _complaintRepository.GetByExpression(x => x.ApartmentId == apartmentId && x.IsSolved == true);
            if (complaint == null)
            {
                return new ComplaintsResponseModel
                {
                    Message = "No solved complaint  for this apartment",
                    Status = false
                };
            }
            var complaintDto = complaint.Select(u => new ComplaintDto
            {
                ComplaintId = u.Id,
                ProblemDescription = u.Problem
                
            }).ToList();
             return new ComplaintsResponseModel
             {
                 Data = complaintDto,
                 Message = "Complaint retrieved",
                 Status = true
             };
       }


       public async Task<ComplaintsResponseModel> GetUnSolvedComplaintsByAparmentId(int apartmentId)
       {
            var complaint = await _complaintRepository.GetByExpression(x => x.ApartmentId == apartmentId && x.IsSolved == false);
            if (complaint == null)
            {
                return new ComplaintsResponseModel
                {
                    Message = "No unsolved complaint  for this apartment",
                    Status = false
                };
            }
            var complaintDto = complaint.Select(u => new ComplaintDto
            {
                ComplaintId = u.Id,
                ProblemDescription = u.Problem
                
            }).ToList();
             return new ComplaintsResponseModel
             {
                 Data = complaintDto,
                 Message = "Complaint retrieved",
                 Status = true
             };
       }

       public async Task<BaseResponse> SolveComplaint(int complaintId)
       {
           var complaint = await _complaintRepository.Get(x => x.Id == complaintId);
           if (complaint == null)
           {
               return new BaseResponse
               {
                   Message = "Complaint not found",
                   Status = false
               };
           }
           var apartment = await _apartmentRepository.GetApartmentInfo(complaint.ApartmentId);
            if (apartment == null)
           {
               return new BaseResponse
               {
                   Message = "Complaint not found",
                   Status = false
               };
           }
           await _apartmentRepository.Update(apartment);
                complaint.IsSolved = true;
                await _complaintRepository.Update(complaint);
           
            return new BaseResponse
            {
                Message = "complaint Successfully created",
                Status = true
            };
       }
       

       public async Task<ComplaintsResponseModel> GetAllUnSolvedComplaints()
       {
            var complaint = await _complaintRepository.GetByExpression(x => x.IsSolved == false);
            if (complaint == null)
            {
                return new ComplaintsResponseModel
                {
                    Message = "No complaint available",
                    Status = false
                };
            }
            var complaintDto = complaint.Select(u => new ComplaintDto
            {
                ComplaintId = u.Id,
                ApartmentNumber = u.ApartmentId.ToString(),
                ProblemDescription = u.Problem
                
            }).ToList();
             return new ComplaintsResponseModel
             {
                 Data = complaintDto,
                 Message = "Complaint retrieved succesful",
                 Status = true
             };
       }
       
       public async Task<DashBoard> ShowDashBoard()
       {
            var unApproveApartmentsCount  = await _apartmentRepository.GetUnApprovedApartments();
            var unRentedApartmentsCount  = await _apartmentRepository.GetUnRentedApartments();
            var unVerifiedCustomerCount  = await _customerRepository.GetNotVerifiedCustomers();
            var unSolvedComplaintsCount  = await _complaintRepository.GetByExpression(x => x.IsSolved == false);
            return new DashBoard
            {
                UnApproveApartmentCount = unApproveApartmentsCount.Count,
                UnRentedApartmentCount = unRentedApartmentsCount.Count,
                UnVerifiedCustomerCount = unVerifiedCustomerCount.Count,
                UnSolvedComplaintsCount = unSolvedComplaintsCount.Count
            };
       }
       
    }
}