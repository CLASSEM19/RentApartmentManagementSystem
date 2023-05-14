using ApartmentRentManagementSystem.Interfaces.Repositories;
using ApartmentRentManagementSystem.Contracts;
using ApartmentRentManagementSystem.Context;
using ApartmentRentManagementSystem.Entities;

namespace ApartmentRentManagementSystem.Implementations.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationContext context)
        {
            _context = context;
        }
        
    }
}