using ApartmentRentManagementSystem.Entities;
using ApartmentRentManagementSystem.Dtos;
namespace ApartmentRentManagementSystem.Interfaces.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
         Task<IList<Category>> GetAllCategories();
    }
}