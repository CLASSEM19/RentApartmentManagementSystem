using ApartmentRentManagementSystem.Interfaces.Repositories;
using ApartmentRentManagementSystem.Contracts;
using ApartmentRentManagementSystem.Context;
using ApartmentRentManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApartmentRentManagementSystem.Implementations.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<Customer> GetCustomerInfo(int id)
        {
            var Customer = await _context.Customers
            .Include(x => x.User).Include(a => a.Address)
            .SingleOrDefaultAsync(x => x.User.Id == id && x.IsDeleted == false);
            return Customer;
        }
        public async Task<Customer> GetCustomerWithUser(int id)
        {
            var Customer = await _context.Customers.Include(x => x.User).Include(a => a.Address).SingleOrDefaultAsync(x => x.UserId == id && x.IsDeleted == false);
            return Customer;
        }
        public async Task<IList<Customer>> GetAllCustomerWithUser()
        {
            var Customer = await _context.Customers.Include(x => x.User).Include(a => a.Address).Where(x => x.IsDeleted == false).ToListAsync();
            return Customer;
        }
        public async Task<IList<Customer>> GetAllVerifiedCustomers()
        {
            var Customer = await _context.Customers.Include(x => x.User).Include(a => a.Address).Where(x => x.IsDeleted == false && x.IsVerified == true).ToListAsync();
            return Customer;
        }
        public async Task<IList<Customer>> GetNotVerifiedCustomers()
        {
            var Customer = await _context.Customers.Include(x => x.User).Include(a => a.Address).Where(x => x.IsDeleted == false && x.IsVerified == false).ToListAsync();
            return Customer;
        }

        public async Task<IList<Apartment>> GetApartmentsByCustomer(int id)
        {
            var apartments = await _context.CustomerApartments.Where(x => x.CustomerId == id).Select(x => new Apartment
            {
                  IsRented = x.Apartment.IsRented,
                 Images = x.Apartment.Images,
                  AddressId = x.Apartment.AddressId,
                  LandlordId = x.Apartment.LandlordId,
                  Period= x.Apartment.Period,
                  TermsAndCondition = x.Apartment.TermsAndCondition,
                  Price = x.Apartment.Price,
                  IsApproved = x.Apartment.IsApproved,
                 HouseEquipments = x.Apartment.HouseEquipments,
                //  Utilities 
                 Category = x.Apartment.Category,
            }).ToListAsync();
            return apartments;
        }
        
    }
}