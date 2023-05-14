using Microsoft.AspNetCore.Mvc;
using ApartmentRentManagementSystem.Interfaces.Services;
using ApartmentRentManagementSystem.Dtos;
using Microsoft.AspNetCore.Hosting;
namespace ApartmentRentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseEquipmentController : ControllerBase
    {
        private readonly IHouseEquipmentService _houseEquipmentService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HouseEquipmentController(IHouseEquipmentService houseEquipmentService , IWebHostEnvironment webHostEnvironment)
        {
            _houseEquipmentService = houseEquipmentService;
            _webHostEnvironment =  webHostEnvironment;
        }
        [HttpPost("AddhouseEquipment")]
        public async Task<IActionResult> AddhouseEquipment(HouseEquipmentRequestModel model)
        {
                var registerhouseEquipment = await _houseEquipmentService.AddNewEquipmentToApartment(model);
               if (registerhouseEquipment.Status == false)
               {
                   return BadRequest(registerhouseEquipment);
               }
                return Ok(registerhouseEquipment);
        }
        
    }
}