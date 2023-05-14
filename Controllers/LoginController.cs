using Microsoft.AspNetCore.Mvc;
using ApartmentRentManagementSystem.Interfaces.Services;
using ApartmentRentManagementSystem.Interfaces.Repositories;
using ApartmentRentManagementSystem.Dtos;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using  ApartmentRentManagementSystem.Authentication;
namespace ApartmentRentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJWTAuthentication _auth;
        private readonly IApartmentRepository _apartmentService;
        private readonly ICustomerService _customerService;
        private readonly IComplaintService _complaintService;
        public LoginController(IUserService userService, IComplaintService complaintService, ICustomerService customerService, IApartmentRepository apartmentService, IJWTAuthentication auth)
        {
            _userService = userService;
            _auth = auth;
            _apartmentService = apartmentService;
            _customerService = customerService;
            _complaintService = complaintService;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserRequestModel model)
        {
                var login = await _userService.Login(model);
               if (!login.Status)
               {
                   return BadRequest(login);
               }
               var token = _auth.GenerateToken(login);
               var response = new LoginResponse
               {
                   Data = login,
                   Token = token
               };
               return Ok(response);
        }
    }
}