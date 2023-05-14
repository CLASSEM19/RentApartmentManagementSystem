using ApartmentRentManagementSystem.Interfaces.Repositories;
using ApartmentRentManagementSystem.Contracts;
using ApartmentRentManagementSystem.Context;
using Microsoft.EntityFrameworkCore;
using ApartmentRentManagementSystem.Entities;
using ApartmentRentManagementSystem.Dtos;

namespace ApartmentRentManagementSystem.Implementations.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IList<Category>> GetAllCategories()
        {
            var categories = await _context.Categories.Where(x => x.IsDeleted == false).ToListAsync();
            return categories;
        }

    }
}