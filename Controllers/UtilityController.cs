using Microsoft.AspNetCore.Mvc;
using ApartmentRentManagementSystem.Interfaces.Services;
using ApartmentRentManagementSystem.Dtos;
using Microsoft.AspNetCore.Hosting;
namespace ApartmentRentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilityController : ControllerBase
    {
        private readonly IUtilityService _utilityService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UtilityController(IUtilityService utilityService , IWebHostEnvironment webHostEnvironment)
        {
            _utilityService = utilityService;
            _webHostEnvironment =  webHostEnvironment;
        }
        [HttpPost("AddUtility")]
        public async Task<IActionResult> AddUtility(UtilityRequestModel model)
        {
                var registerutility = await _utilityService.AddUtility(model);
               if (registerutility.Status == false)
               {
                   return BadRequest(registerutility);
               }
                return Ok(registerutility);
        }
        
    }
}