using ApartmentRentManagementSystem.Entities;
using ApartmentRentManagementSystem.Dtos;
namespace ApartmentRentManagementSystem.Interfaces.Repositories
{
    public interface IImageRepository : IRepository<Image>
    {
        IList<ImageDto> GetImagesByApartmentId(int id);
    }

}