using Microsoft.AspNetCore.Mvc;
using ApartmentRentManagementSystem.Interfaces.Services;
using ApartmentRentManagementSystem.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text.Json;
// using Newtonsoft.Json;
namespace ApartmentRentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentController : ControllerBase
    {
        private readonly IApartmentService _apartmentService;
        private readonly IHouseEquipmentService _houseEquipmentService;
        private readonly  IImageService _imageService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ApartmentController(IApartmentService apartmentService, IHouseEquipmentService houseEquipmentService, IImageService imageService, IWebHostEnvironment webHostEnvironment)
        {
            _apartmentService = apartmentService;
            _webHostEnvironment =  webHostEnvironment;
            _imageService =  imageService;
            _houseEquipmentService =  houseEquipmentService;
        }
        [Authorize]
        [HttpPost("RegisterApartment")]
        public async Task<IActionResult> RegisterApartment([FromForm]ApartmentRequestModel model)
        { 
            var getUser = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userId = int.Parse(getUser);
            model.LandlordId = userId;
            Console.WriteLine($"Controller Line 32 {model.LandlordId}");
            var files = HttpContext.Request.Form;

            if (files != null && files.Count > 0)
            {
                string imageDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                Directory.CreateDirectory(imageDirectory);
                foreach (var file in files.Files)
                {
                    FileInfo info = new FileInfo(file.FileName);
                    string image = Guid.NewGuid().ToString() + info.Extension;
                    string path = Path.Combine(imageDirectory, image);
                    using(var filestream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    model.Images.Add(image);
                }
            }
                var registerApartment = await _apartmentService.RegisterApartment(model);
               if (!registerApartment.Status)
               {
                   return BadRequest(registerApartment);
               }
                return Ok(registerApartment);
        }
        
        [HttpPut("UpdateApartment/{id}")]
        public async Task<IActionResult> UpdateApartment([FromForm]UpdateApartmentRequestModel model,[FromRoute] int id)
        {
             var files = HttpContext.Request.Form;
            if (files != null && files.Count > 0)
            {
                string imageDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                Directory.CreateDirectory(imageDirectory);
                foreach (var file in files.Files)
                {
                    FileInfo info = new FileInfo(file.FileName);
                    string image = Guid.NewGuid().ToString() + info.Extension;
                    string path = Path.Combine(imageDirectory, image);
                    using(var filestream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    
                        model.Images.Add(image);

                }  
            }
            var apartment = await _apartmentService.UpdateApartment(model, id);
            if(apartment.Status)
            {
                return Ok(apartment);
            }
            return BadRequest(apartment);
        }

        [HttpPut("AddCategoryToApartment")]
        public async Task<IActionResult> AddCategoryToApartment(ApartmentCategory model)
        {
            var apartment = await _apartmentService.AddCategoryToApartment(model);
            if(apartment.Status)
            {
                return Ok(apartment);
            }
            return BadRequest(apartment);
        }

        [HttpGet("GetApartmentInformation/{id}")]
       public async Task<IActionResult> GetApartmentById([FromRoute]int id)
       {
           var apartment = await _apartmentService.GetApartmentById(id);
           if(apartment.Status)
           {
               return Ok(apartment);
           }
           return BadRequest(apartment);
       }

            [HttpPut("ApproveApartment/{id}")]
       public async Task<IActionResult> ApproveApartment([FromRoute]int id)
       {
           var apartment = await _apartmentService.ApproveApartment(id);
           if(apartment.Status)
           {
               return Ok(apartment);
           }
           return BadRequest(apartment);
       }

        [HttpGet("GetApprovedApartments")]
       public async Task<IActionResult> GetApprovedApartments()
       {
           var apartment = await _apartmentService.GetApprovedApartments();
           if(apartment.Status)
           {
               return Ok(apartment);
           }
           return BadRequest(apartment);
       }

        [HttpPost("DisApproveApartment")]
       public async Task<IActionResult> DisApproveApartment(int id)
       {
           var apartment = await _apartmentService.DisApproveApartment(id);
           if(apartment.Status)
           {
               return Ok(apartment);
           }
           return BadRequest(apartment);
       }

        [HttpGet("GetUnApprovedApartments")]
       public async Task<IActionResult> GetUnApprovedApartments()
       {
           var apartment = await _apartmentService.GetUnApprovedApartments();
           if(apartment.Status)
           {
               return Ok(apartment);
           }
           return BadRequest(apartment);
       }

        [HttpPut("RentApartment/{id}/{customerId}")]
       public async Task<IActionResult> RentApartment([FromRoute]int id, [FromRoute]int customerId)
       {
           var apartment = await _apartmentService.RentApartment(id, customerId);
           if(apartment.Status)
           {
               return Ok(apartment);
           }
           return BadRequest(apartment);
       }

        [HttpGet("GetAllRentedApartments")]
       public async Task<IActionResult> GetRentedApartments()
       {
           var apartment = await _apartmentService.GetRentedApartments();
           if(apartment.Status)
           {
               return Ok(apartment);
           }
           return BadRequest(apartment);
       }

        [HttpPost("UnRentApartment")]
       public async Task<IActionResult> UnRentApartment(int id)
       {
           var apartment = await _apartmentService.UnRentApartment(id);
           if(apartment.Status)
           {
               return Ok(apartment);
           }
           return BadRequest(apartment);
       }


        [HttpGet("GetAllUnRentedApartments")]
       public async Task<IActionResult> GetAllUnRentedApartments()
       {
           var apartment = await _apartmentService.GetUnRentedApartments();
           if(apartment.Status)
           {
               return Ok(apartment);
           }
           return BadRequest(apartment);
       }

    
         [HttpGet("DeleteEquipmentFromApartment")]
       public async Task<IActionResult> DeleteEquipmentFromApartment(int id, string name)
       {
           var apartment = await _houseEquipmentService.DeleteEquipmentFromApartment(id, name);
           if(apartment.Status)
           {
               return Ok(apartment);
           }
           return BadRequest(apartment);
       }

        [HttpGet("GetApartmentsByCountry")]
       public async Task<IActionResult> GetApartmentsByCountry(string country)
       {
           var apartment = await _apartmentService.GetApartmentsByCountry(country);
           if(apartment.Status)
           {
               return Ok(apartment);
           }
           return BadRequest(apartment);
       }

        [HttpGet("GetApartmentsByState")]
       public async Task<IActionResult> GetApartmentsByState(string state)
       {
           var apartment = await _apartmentService.GetApartmentsByState(state);
           if(apartment.Status)
           {
               return Ok(apartment);
           }
           return BadRequest(apartment);
       }


        [HttpGet("GetApartmentsByLGA")]
       public async Task<IActionResult> GetApartmentsByLGA(string LGA)
       {
           var apartment = await _apartmentService.GetApartmentsByLGA(LGA);
           if(apartment.Status)
           {
               return Ok(apartment);
           }
           return BadRequest(apartment);
       }

        [HttpGet("GetAllApartments")]
       public async Task<IActionResult> GetAllApartments()
       {
           var apartment = await _apartmentService.GetAllApartments();
           if(apartment.Status)
           {
               return Ok(apartment);
           }
           return BadRequest(apartment);
       }
       
    }
}