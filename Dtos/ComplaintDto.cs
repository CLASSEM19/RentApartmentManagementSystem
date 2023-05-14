namespace ApartmentRentManagementSystem.Dtos
{
    public class ComplaintDto
    {
        public int ComplaintId{get; set;}
        public string ProblemDescription{get; set;}
        public decimal SolutionFee{get; set;}        
        public string ApartmentNumber{get; set;}
    }
    public class DashBoard
    {
        public int UnApproveApartmentCount{get; set;}
        public int UnRentedApartmentCount{get; set;}
        public int UnSolvedComplaintsCount{get; set;}        
        public int UnVerifiedCustomerCount{get; set;}
    }
    public class ComplaintRequestModel
    {
        public string ProblemDescription{get; set;}
        public string ApartmentNumber{get; set;}
    }
    public class ComplaintsResponseModel : BaseResponse
    {
        public ICollection<ComplaintDto> Data{get; set;} = new HashSet<ComplaintDto>();
    }
}