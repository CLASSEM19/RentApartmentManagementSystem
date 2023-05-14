using ApartmentRentManagementSystem.Entities;
namespace ApartmentRentManagementSystem.Interfaces.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
         Task<Customer> GetCustomerInfo(int id);

        Task<IList<Customer>> GetAllCustomerWithUser();

        Task<IList<Apartment>> GetApartmentsByCustomer(int id);

        Task<IList<Customer>> GetAllVerifiedCustomers();
        
        Task<IList<Customer>> GetNotVerifiedCustomers();
    }
}