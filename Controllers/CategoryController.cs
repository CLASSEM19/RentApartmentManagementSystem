using Microsoft.AspNetCore.Mvc;
using ApartmentRentManagementSystem.Interfaces.Services;
using ApartmentRentManagementSystem.Dtos;
using Microsoft.AspNetCore.Hosting;
namespace ApartmentRentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CategoryController(ICategoryService categoryService , IWebHostEnvironment webHostEnvironment)
        {
            _categoryService = categoryService;
            _webHostEnvironment =  webHostEnvironment;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterCategory(CategoryRequestModel model)
        {
                var registerCategory = await _categoryService.RegisterCategory(model);
               if (!registerCategory.Status)
               {
                   return BadRequest(registerCategory);
               }
                return Ok(registerCategory);
        }
        
        
        [HttpGet("GetAllCategorys")]
        public async Task<IActionResult> GetAllCategorys()
        {
            var Category = await _categoryService.GetAllCategories();
            return Ok(Category);
        }
    }
}