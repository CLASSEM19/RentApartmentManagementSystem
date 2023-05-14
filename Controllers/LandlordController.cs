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
    public class LandlordController : ControllerBase
    {
        private readonly ILandlordService _landlordService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public LandlordController(ILandlordService landlordService , IWebHostEnvironment webHostEnvironment)
        {
            _landlordService = landlordService;
            _webHostEnvironment =  webHostEnvironment;
        }
        [HttpPost("RegisterLandlord")]
        public async Task<IActionResult> RegisterLandlord([FromForm]LandlordRequestModel model)
        {
                var registerLandlord = await _landlordService.RegisterLandlord(model);
               if (registerLandlord.Status == false)
               {
                   return BadRequest(registerLandlord);
               }
                return Ok(registerLandlord);
        }
        [HttpPut("UpdateLandlord/{id}")]
        public async Task<IActionResult> UpdateLandlord([FromForm]UpdateLandlordRequestModel model, [FromRoute]int id)
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
                    model.Image = (image);
                }
            }
            var landlord = await _landlordService.UpdateLandlord(model, id);
            if (!landlord.Status)
            {
                return BadRequest(landlord);
            }
            return Ok(landlord);
        }

        [HttpGet("GetLandLord")]
        public async Task<IActionResult> GetLandLord(string email)
        {
            var landlord = await _landlordService.GetLandLordByEmail(email);
            if (landlord.Status)
            {
                return Ok(landlord);
            }
            return BadRequest(landlord);
        }


        [HttpGet("GetLandLordById/{id}")]
        public async Task<IActionResult> GetLandLordById([FromRoute]int id)
        {
            
            var landlord = await _landlordService.GetLandLordById(id);
            if (landlord.Status)
            {
                return Ok(landlord);
            }
            return BadRequest(landlord);
        }

        [HttpGet("GetLandLordInfo/{id}")]
        public async Task<IActionResult> GetLandLordInfo([FromRoute]int id)
        {
            
            var landlord = await _landlordService.GetLandLordInfo(id);
            if (landlord.Status)
            {
                return Ok(landlord);
            }
            return BadRequest(landlord);
        }
        [HttpGet("GetRentedApartmentsByLandlord/{id}")]
        public async Task<IActionResult> GetRentedApartmentsByLandlord([FromRoute]int id)
        {
            if (id == 0)
            {
               var getUser = User.FindFirst(ClaimTypes.NameIdentifier).Value;
               var userId = int.Parse(getUser);
                id = userId;
            }
            var landlord = await _landlordService.GetRentedApartmentsByLandlord(id);
            if (landlord.Status)
            {
                return Ok(landlord);
            }
            return BadRequest(landlord);
        }

        [HttpGet("GetApartmentsByLandlord/{id}")]
        public async Task<IActionResult> GetApartmentsByLandlord([FromRoute]int id)
        {
            Console.WriteLine($"controller id= {id}");
            if (id == 0)
            {
               Console.WriteLine($"controller Seen= {id}");
               var getUser = User.FindFirst(ClaimTypes.NameIdentifier).Value;
               Console.WriteLine($"controller getUser= {getUser}");
               var userId = int.Parse(getUser);
               Console.WriteLine($"controller userId= {userId}");
                id = userId;
            }
            Console.WriteLine($"controller id= {id}");
            var landlord = await _landlordService.GetApartmentsByLandlord(id);
            if (landlord.Status)
            {
                return Ok(landlord);
            }
            return BadRequest(landlord);
        }

        [HttpGet("GetApartmentsByUserId/{id}")]
        public async Task<IActionResult> GetApartmentsByUserId([FromRoute]int id)
        {
            var landlord = await _landlordService.GetApartmentsByUserId(id);
            if (landlord.Status)
            {
                return Ok(landlord);
            }
            return BadRequest(landlord);
        }
        [HttpGet("GetUnApprovedApartmentsByLandlord/{id}")]
        public async Task<IActionResult> GetUnApprovedApartmentsByLandlord([FromRoute]int id)
        {
           if (id == 0)
            {
               var getUser = User.FindFirst(ClaimTypes.NameIdentifier).Value;
               var userId = int.Parse(getUser);
                id = userId;
            }
            var landlord = await _landlordService.GetUnApprovedApartmentsByLandlord(id);
            if (landlord.Status)
            {
                return Ok(landlord);
            }
            return BadRequest(landlord);
        }

        [HttpGet("GetApprovedApartmentsByLandlord")]
        public async Task<IActionResult> GetApprovedApartmentsByLandlord([FromRoute]int id)
        {
            if (id == 0)
            {
               var getUser = User.FindFirst(ClaimTypes.NameIdentifier).Value;
               var userId = int.Parse(getUser);
                id = userId;
            }
            var landlord = await _landlordService.GetApprovedApartmentsByLandlord(id);
            if (landlord.Status)
            {
                return Ok(landlord);
            }
            return BadRequest(landlord);
        }
        [HttpGet("GetUnRentedApartmentsByLandlord/{id}")]
        public async Task<IActionResult> GetUnRentedApartmentsByLandlord([FromRoute]int id)
        {
            if (id == 0)
            {
               var getUser = User.FindFirst(ClaimTypes.NameIdentifier).Value;
               var userId = int.Parse(getUser);
                id = userId;
            }
            var landlord = await _landlordService.GetUnRentedApartmentsByLandlord(id);
            if (landlord.Status)
            {
                return Ok(landlord);
            }
            return BadRequest(landlord);
        }
        [HttpPost("Activate")]
        public async Task<IActionResult> ActivateLandlord(int id)
        {
            var landlord = await _landlordService.ActivateLandlord(id);
            if(landlord.Status)
            {
                return Ok(landlord);
            }
            return BadRequest(landlord);
        }
        [HttpPost("Deactivate")]
        public async Task<IActionResult> DeActivateLandlord(int id)
        {
            var landlord = await _landlordService.DeActivateLandlord(id);
            if(landlord.Status)
            {
                return Ok(landlord);
            }
            return BadRequest(landlord);
        }

        [HttpGet("GetAllActivatedLandlords")]
        public async Task<IActionResult> GetAllActivatedLandlords()
        {
            var landlord = await _landlordService.GetAllActivatedLandlords();
            return Ok(landlord);
        }

        [HttpGet("GetAllDeactivatedLandlords")]
        public async Task<IActionResult> GetAllDeactivatedLandlords()
        {
            var landlord = await _landlordService.GetAllDeactivatedLandlords();
            return Ok(landlord);
        }

        [HttpGet("GetAllLAndlords")]
        public async Task<IActionResult> GetAllLandlords()
        {
            var landlord = await _landlordService.GetAllLandlords();
            return Ok(landlord);
        }
    }
}