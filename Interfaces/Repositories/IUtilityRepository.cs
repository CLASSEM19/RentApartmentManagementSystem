using ApartmentRentManagementSystem.Entities;
using ApartmentRentManagementSystem.Dtos;
namespace ApartmentRentManagementSystem.Interfaces.Repositories
{
    public interface IUtilityRepository : IRepository<Utility>
    {
         
        IList<UtilityDto> GetUtilitysByApartmentId(int id);
    }

}