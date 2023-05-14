using Microsoft.AspNetCore.Mvc;
using ApartmentRentManagementSystem.Interfaces.Services;
using ApartmentRentManagementSystem.Dtos;
using Microsoft.AspNetCore.Hosting;
namespace ApartmentRentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintController : ControllerBase
    {
        private readonly IComplaintService _complaintService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ComplaintController(IComplaintService complaintService , IWebHostEnvironment webHostEnvironment)
        {
            _complaintService = complaintService;
            _webHostEnvironment =  webHostEnvironment;
        }
        [HttpPost("MakeComplaint")]
        public async Task<IActionResult> MakeComplaint(ComplaintRequestModel model)
        {
                var registercomplaint = await _complaintService.MakeComplaint(model);
               if (registercomplaint.Status == false)
               {
                   return BadRequest(registercomplaint);
               }
                return Ok(registercomplaint);
        }
        [HttpGet("GetSolvedComplaintsByAparmentId/{apartmentId}")]
        public async Task<IActionResult> GetSolvedComplaintsByAparmentId([FromRoute]int apartmentId)
        {
                var registercomplaint = await _complaintService.GetSolvedComplaintsByAparmentId(apartmentId);
               if (registercomplaint.Status == false)
               {
                   return BadRequest(registercomplaint);
               }
                return Ok(registercomplaint);
        }

        [HttpGet("GetUnSolvedComplaintsByAparmentId/{apartmentId}")]
        public async Task<IActionResult> GetUnSolvedComplaintsByAparmentId([FromRoute]int apartmentId)
        {
                var registercomplaint = await _complaintService.GetUnSolvedComplaintsByAparmentId(apartmentId);
               if (registercomplaint.Status == false)
               {
                   return BadRequest(registercomplaint);
               }
                return Ok(registercomplaint);
        }


        [HttpPut("SolveComplaint/{complaintId}")]
        public async Task<IActionResult> SolveComplaint(int complaintId)
        {
                var registercomplaint = await _complaintService.SolveComplaint(complaintId);
               if (registercomplaint.Status == false)
               {
                   return BadRequest(registercomplaint);
               }
                return Ok(registercomplaint);
        }


        [HttpGet("GetAllUnSolvedComplaints")]
        public async Task<IActionResult> GetAllUnSolvedComplaints()
        {
            var registercomplaint = await _complaintService.GetAllUnSolvedComplaints();
            System.Console.WriteLine(registercomplaint.Data);
            return Ok(registercomplaint);
        }
        [HttpGet("ShowDashBoard")]
        public async Task<IActionResult> ShowDashBoard()
        {
            var registercomplaint = await _complaintService.ShowDashBoard();
            return Ok(registercomplaint);
        }
        
    }
}