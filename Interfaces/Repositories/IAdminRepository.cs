using ApartmentRentManagementSystem.Entities;
namespace ApartmentRentManagementSystem.Interfaces.Repositories
{
    public interface IAdminRepository : IRepository<Admin>
    {

        Task<Admin> GetAdminInfo(int id);

        Task<IList<Admin>> GetAllAdminWithUser();
        
        Task<IList<Admin>> GetAllActivatedAdmin();
        
        Task<IList<Admin>> GetAllDeactivatedAdmin();
    }

    
}