using Microsoft.AspNetCore.Mvc;
using ApartmentRentManagementSystem.Interfaces.Services;
using ApartmentRentManagementSystem.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
namespace ApartmentRentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(IAdminService adminService , IWebHostEnvironment webHostEnvironment)
        {
            _adminService = adminService;
            _webHostEnvironment =  webHostEnvironment;
        }
        [HttpPost("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromForm]AdminRequestModel model)
        {
                var registerAdmin = await _adminService.RegisterAdmin(model);
               if (registerAdmin.Status == false)
               {
                   return BadRequest(registerAdmin);
               }
                return Ok(registerAdmin);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAdmin(UpdateAdminRequestModel model)
        {
            var getUser = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userId = int.Parse(getUser);
            var Admin = await _adminService.UpdateAdmin(model, userId);
            if (!Admin.Status)
            {
                return BadRequest(Admin);
            }
            return Ok(Admin);
        }

        [HttpGet("GetAdmin")]
        public async Task<IActionResult> GetAdmin(int id)
        {
            var Admin = await _adminService.GetAdminInfo(id);
            if (Admin.Status)
            {
                return Ok(Admin);
            }
            return BadRequest(Admin);
        }


        [HttpGet("GetAdminById/{id}")]
        public async Task<IActionResult> GetAdminById([FromRoute]int id)
        {
            if (id == 0)
            {
               var getUser = User.FindFirst(ClaimTypes.NameIdentifier).Value;
               var userId = int.Parse(getUser);
                id = userId;
            }
            var Admin = await _adminService.GetAdminInfo(id);
            if (Admin.Status)
            {
                return Ok(Admin);
            }
            return BadRequest(Admin);
        }
        [HttpPost("Activate")]
        public async Task<IActionResult> ActivateAdmin(int id)
        {
            var Admin = await _adminService.ActivateAdmin(id);
            if(Admin.Status)
            {
                return Ok(Admin);
            }
            return BadRequest(Admin);
        }
        [HttpPost("Deactivate")]
        public async Task<IActionResult> DeActivateAdmin(int id)
        {
            var Admin = await _adminService.DeActivateAdmin(id);
            if(Admin.Status)
            {
                return Ok(Admin);
            }
            return BadRequest(Admin);
        }

        [HttpGet("GetAllActivatedAdmins")]
        public async Task<IActionResult> GetAllActivatedAdmins()
        {
            var Admin = await _adminService.GetAllActivatedAdmins();
            return Ok(Admin);
        }

        [HttpGet("GetAllDeactivatedAdmins")]
        public async Task<IActionResult> GetAllDeactivatedAdmins()
        {
            var Admin = await _adminService.GetAllDeactivatedAdmins();
            return Ok(Admin);
        }

        [HttpGet("GetAllAdmins")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var Admin = await _adminService.GetAllAdmins();
            return Ok(Admin);
        }
    }
}