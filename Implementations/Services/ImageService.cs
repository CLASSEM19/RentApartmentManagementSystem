using System;
using System;
using ApartmentRentManagementSystem.Dtos;
using ApartmentRentManagementSystem.Interfaces.Repositories;
using ApartmentRentManagementSystem.Entities;
using ApartmentRentManagementSystem.Interfaces.Services;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace ApartmentRentManagementSystem.Implementations.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        public async Task<BaseResponse> RegisterImage(string model)
        {
            if (model == null)
            {
                return new BaseResponse
                {
                    Status = false,
                    Message = "Value cannot be nuull"
                };
            }
            
            var cate = new Image
            {
                Path = model
            };
            await _imageRepository.Register(cate);
            return new BaseResponse
            {
                Status = true,
                Message = "New Image succussfully added"
            };
        }
    }
}