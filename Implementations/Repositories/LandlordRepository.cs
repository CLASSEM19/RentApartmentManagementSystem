using ApartmentRentManagementSystem.Interfaces.Repositories;
using ApartmentRentManagementSystem.Contracts;
using ApartmentRentManagementSystem.Context;
using ApartmentRentManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;


namespace ApartmentRentManagementSystem.Implementations.Repositories
{
    public class LandlordRepository : BaseRepository<Landlord>, ILandlordRepository
    {
        public LandlordRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<Landlord> GetLandLordWithUser(string email)
        {
            var landlord = await _context.Landlords.Include(x => x.User).Include(a => a.Address).SingleOrDefaultAsync(x => x.User.Email == email && x.IsDeleted == false);
            return landlord;
        }

        public async Task<Landlord> GetLandLordWithUser(int id)
        {
            var landlord = await _context.Landlords.Include(x => x.User).Include(a => a.Address).SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            return landlord;
        }
        public async Task<Landlord> GetLandLordInfo(int id)
        {
            var landlord = await _context.Landlords.Include(x => x.User).Include(a => a.Address).SingleOrDefaultAsync(x => x.UserId == id && x.IsDeleted == false);
            return landlord;
        }
        public async Task<IList<Landlord>> GetAllLandLordWithUser()
        {
            var landlord = await _context.Landlords.Include(x => x.User).Include(a => a.Address).Where(x => x.IsDeleted == false).ToListAsync();
            return landlord;
        }

        public async Task<IList<Apartment>> GetApartmentsByLandlord(int id)
        {
            Console.WriteLine($"Repository {id}");
            var apartments = await _context.Apartments
            .Include(x => x.Address)
            .Include(a => a.Images)
            .Include(b => b.HouseEquipments)
            .Include(b => b.Utilities)
            .Include(c => c.Category)
            .Where(x => x.LandlordId == id).ToListAsync();
            Console.WriteLine(apartments);
            return apartments;
        }
        public async Task<IList<Apartment>> GetApprovedApartmentsByLandlord(int id)
        {
            var apartments = await _context.Apartments.Where(x => x.LandlordId == id && x.IsApproved == true).ToListAsync();
            return apartments;
        }
        public async Task<IList<Apartment>> GetUnApprovedApartmentsByLandlord(int id)
        {
            var apartments = await _context.Apartments.Where(x => x.LandlordId == id && x.IsApproved == false).ToListAsync();
            return apartments;
        }
        public async Task<IList<Apartment>> GetUnRentedApartmentsByLandlord(int id)
        {
            var apartments = await _context.Apartments.Where(x => x.LandlordId == id && x.IsRented == false).ToListAsync();
            return apartments;
        }
        public async Task<IList<Apartment>> GetRentedApartmentsByLandlord(int id)
        {
            var apartments = await _context.Apartments.Where(x => x.LandlordId == id && x.IsRented == true).ToListAsync();
            return apartments;
        }
        public async Task<IList<Landlord>> GetAllActivatedLandlord()   
        {
            var landlord = await _context.Landlords.Where(x => x.IsDeleted == false).ToListAsync();
            return landlord;
        }
        public async Task<IList<Landlord>> GetAllDeactivatedLandlord()
        {
            var landlord = await _context.Landlords.Where(x => x.IsDeleted == true).ToListAsync();
            return landlord;
        }
        // public async Task<IList<Apartment>> GetApartmentsByLandlord(int id)
        // {
        //     var apartments = await _context.Apartments.Where(x => x.Id == id).Include(x => x.Landlord).Select(l => new Landlord);
        //     return apartments;
        // }
    }
}