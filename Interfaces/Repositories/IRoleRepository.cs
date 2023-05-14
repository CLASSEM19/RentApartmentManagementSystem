using ApartmentRentManagementSystem.Contracts;
using ApartmentRentManagementSystem.Identity;
namespace ApartmentRentManagementSystem.Interfaces.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
         IList<Role> GetRolesByUserId(int id);
    }
}