using ApartmentRentManagementSystem.Interfaces.Repositories;
using ApartmentRentManagementSystem.Contracts;
using ApartmentRentManagementSystem.Context;
using Microsoft.EntityFrameworkCore;
using ApartmentRentManagementSystem.Entities;

namespace ApartmentRentManagementSystem.Implementations.Repositories
{
    public class ApartmentRepository : BaseRepository<Apartment>, IApartmentRepository
    {
        public ApartmentRepository(ApplicationContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// This Get The information about an apartment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Apartment> GetApartmentInfo(int id)
        {
            var apart = await _context.Apartments
            .Include(x => x.Address)
            .Include(a => a.Images)
            .Include(b => b.HouseEquipments)
            .Include(b => b.Utilities)
            .Include(c => c.Category).SingleOrDefaultAsync(e => e.Id == id);
            return apart;
        }
        public async Task<IList<Apartment>> GetApartmentByRenterId(int id)
        {
            var apart = await _context.Apartments
            .Include(x => x.Address)
            .Include(a => a.Images)
            .Include(b => b.HouseEquipments)
            .Include(b => b.Utilities)
            .Include(c => c.Category).Where(e => e.RentBy == id)
            .ToListAsync();
            return apart;
        }

        public async Task<IList<Apartment>> GetRentedApartments()
        {
            var apart = await _context.Apartments.Include(x => x.Address)
            .Include(a => a.Images)
            .Include(b => b.HouseEquipments)
            .Include(b => b.Utilities)
            .Include(d => d.Category)
            .Where(e => e.IsRented == true && e.IsApproved == true).ToListAsync();
            return apart;
        }

        public async Task<IList<Apartment>> GetAllApartments()
        {
            var apart = await _context.Apartments.Include(x => x.Address)
            .Include(a => a.Images)
            .Include(b => b.HouseEquipments)
            .Include(b => b.Utilities)
            .Include(d => d.Category).ToListAsync();
            return apart;
        }


        public async Task<IList<Apartment>> GetUnRentedApartments()
        {
            var apart = await _context.Apartments.Include(x => x.Address)
            .Include(a => a.Images)
            .Include(b => b.HouseEquipments)
            .Include(b => b.Utilities)
            .Include(d => d.Category)
            .Where(e => e.IsRented == false && e.IsApproved == true).ToListAsync();
            return apart;
        }


        public async Task<IList<Apartment>> GetApprovedApartments()
        {
            var apart = await _context.Apartments.Include(x => x.Address)
            .Include(a => a.Images)
            .Include(b => b.HouseEquipments)
            .Include(b => b.Utilities)
            .Include(d => d.Category)
            .Where(e => e.IsApproved == true).ToListAsync();
            return apart;
        }


        public async Task<IList<Apartment>> GetUnApprovedApartments()
        {
            var apart = await _context.Apartments.Include(x => x.Address)
            .Include(a => a.Images)
            .Include(b => b.HouseEquipments)
            .Include(b => b.Utilities)
            .Where(e => e.IsApproved == false).ToListAsync();
            return apart;
        }

        public async Task<IList<Apartment>> GetApartmentsByState(string state)
        {
            var apartments = await _context.Apartments.Include(x => x.Address)
            .Where(x => x.Address.State.ToUpper() == state.ToUpper())
            .ToListAsync();
            return apartments;
        }

        public async Task<IList<Apartment>> GetApartmentsByCountry(string country)
        {
            var apartments = await _context.Apartments.Include(x => x.Address)
            .Where(x => x.Address.Country.ToUpper() == country.ToUpper())
            .ToListAsync();
            return apartments;
        }

        public async Task<IList<Apartment>> GetApartmentsByLGA(string lga)
        {
            var apartments = await _context.Apartments.Include(x => x.Address)
            .Where(x => x.Address.LGA.ToUpper() == lga.ToUpper())
            .ToListAsync();
            return apartments;
        }
        
   }
}