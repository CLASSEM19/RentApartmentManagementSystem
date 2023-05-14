using ApartmentRentManagementSystem.Interfaces.Repositories;
using ApartmentRentManagementSystem.Contracts;
using ApartmentRentManagementSystem.Context;
using ApartmentRentManagementSystem.Entities;
using ApartmentRentManagementSystem.Dtos;

namespace ApartmentRentManagementSystem.Implementations.Repositories
{
    public class ImageRepository : BaseRepository<Image>, IImageRepository
    {
        public ImageRepository(ApplicationContext context)
        {
            _context = context;
        }
        
        public IList<ImageDto> GetImagesByApartmentId(int id)
        {
           var imgs = _context.Images.Where(x => x.ApartmentId == id).Select(d => new ImageDto
           {
               Name = d.Path
           }).ToList();
           return imgs;
        }
    }
}