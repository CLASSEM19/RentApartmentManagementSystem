using ApartmentRentManagementSystem.Contracts;
using ApartmentRentManagementSystem.Context;
using ApartmentRentManagementSystem.Entities;
using ApartmentRentManagementSystem.Identity;
using Microsoft.EntityFrameworkCore;
using ApartmentRentManagementSystem.Interfaces.Repositories;
namespace ApartmentRentManagementSystem.Implementations.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationContext context)
        {
            _context = context;
        }

        public IList<Role> GetRolesByUserId(int id)
        {
            var roles = _context.UserRole.Include(r => r.Role).Where(x => x.UserId == id).Select(r => new Role
            {
                 Name = r.Role.Name,
                 Description =  r.Role.Description
            }).ToList();
            return roles;
        }
    }
}