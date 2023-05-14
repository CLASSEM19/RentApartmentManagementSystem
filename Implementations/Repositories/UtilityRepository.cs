using ApartmentRentManagementSystem.Interfaces.Repositories;
using ApartmentRentManagementSystem.Contracts;
using ApartmentRentManagementSystem.Context;
using ApartmentRentManagementSystem.Entities;
using ApartmentRentManagementSystem.Dtos;

namespace ApartmentRentManagementSystem.Implementations.Repositories
{
    public class UtilityRepository : BaseRepository<Utility>, IUtilityRepository
    {
        public UtilityRepository(ApplicationContext context)
        {
            _context = context;
        }
        

        public IList<UtilityDto> GetUtilitysByApartmentId(int id)
        {
           var Utilities = _context.Utilities.Where(x => x.ApartmentId == id).Select(d => new UtilityDto
           {
                Name = d.Name,
                Quantity = d.Number,
                Description = d.Description
           }).ToList();
           return Utilities;
        }
    }
}