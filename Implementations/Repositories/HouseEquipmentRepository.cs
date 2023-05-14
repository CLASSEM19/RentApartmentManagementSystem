using ApartmentRentManagementSystem.Interfaces.Repositories;
using ApartmentRentManagementSystem.Contracts;
using ApartmentRentManagementSystem.Context;
using ApartmentRentManagementSystem.Entities;

namespace ApartmentRentManagementSystem.Implementations.Repositories
{
    public class HouseEquipmentRepository : BaseRepository<HouseEquipment>, IHouseEquipmentRepository
    {
        public HouseEquipmentRepository(ApplicationContext context)
        {
            _context = context;
        }
        
    }
}