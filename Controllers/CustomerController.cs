using Microsoft.AspNetCore.Mvc;
using ApartmentRentManagementSystem.Interfaces.Services;
using ApartmentRentManagementSystem.Dtos;
using Microsoft.AspNetCore.Hosting;
namespace ApartmentRentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    // [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CustomerController(ICustomerService customerService , IWebHostEnvironment webHostEnvironment)
        {
            _customerService = customerService;
            _webHostEnvironment =  webHostEnvironment;
        }
        [HttpPost("RegisterCustomer")]
        public async Task<IActionResult> RegisterCustomer(CustomerRequestModel model)
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

                var registercustomer = await _customerService.RegisterCustomer(model);
               if (registercustomer.Status == false)
               {
                   return BadRequest(registercustomer);
               }
                return Ok(registercustomer);
        }
        [HttpPut("UpdateCustomer/{id}")]
        public async Task<IActionResult> UpdateCustomer(UpdateCustomerRequestModel model,[FromRoute] int id)
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
            var customer = await _customerService.UpdateCustomer(model, id);
            if (!customer.Status)
            {
                return BadRequest(customer);
            }
            return Ok(customer);
        }

        [HttpGet("GetCustomerById/{id}")]
        public async Task<IActionResult> GetCustomerById([FromRoute]int id)
        {
            var customer = await _customerService.GetCustomerInfo(id);
            if (customer.Status)
            {
                return Ok(customer);
            }
            return BadRequest(customer);
        }
    
        [HttpGet("GetAllcustomers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customer = await _customerService.GetAllCustomers();
            return Ok(customer);
        }

        [HttpGet("GetApartmentsByCustomerId/{id}")]
        public async Task<IActionResult> GetApartmentsByCustomerId([FromRoute]int id)
        {
            var customers = await _customerService.GetApartmentsByCustomerId(id);
            if (customers.Status)
            {
                return Ok(customers);
            }
            return BadRequest(customers);
        }

        [HttpPut("VerifyCustomer/{id}")]
        public async Task<IActionResult> VerifyCustomer([FromRoute]int id)
        {
            var customer = await _customerService.VerifyCustomer(id);
            if (customer != null)
            {
                return Ok(customer);
            }
            return BadRequest(customer);
        }

        [HttpGet("GetAllVerifiedCustomers")]
        public async Task<IActionResult> GetAllVerifiedCustomers()
        {
            var customer = await _customerService.GetAllVerifiedCustomers();
            return Ok(customer);
        }

        [HttpGet("GetNotVerifiedCustomers")]
        public async Task<IActionResult> GetNotVerifiedCustomers()
        {
            var customer = await _customerService.GetNotVerifiedCustomers();
            return Ok(customer);
        }

    }
}