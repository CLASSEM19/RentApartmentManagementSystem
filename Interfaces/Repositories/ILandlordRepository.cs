using ApartmentRentManagementSystem.Entities;
using System.Threading.Tasks;
namespace ApartmentRentManagementSystem.Interfaces.Repositories
{
    public interface ILandlordRepository : IRepository<Landlord>
    {
        Task<Landlord> GetLandLordWithUser(string email);

        Task<Landlord> GetLandLordWithUser(int id);

        Task<IList<Landlord>> GetAllLandLordWithUser();

        Task<IList<Apartment>> GetApartmentsByLandlord(int id);

        Task<IList<Landlord>> GetAllActivatedLandlord();

        Task<IList<Landlord>> GetAllDeactivatedLandlord();

        Task<IList<Apartment>> GetApprovedApartmentsByLandlord(int id);

        Task<IList<Apartment>> GetUnApprovedApartmentsByLandlord(int id);

        Task<IList<Apartment>> GetUnRentedApartmentsByLandlord(int id);
        
        Task<IList<Apartment>> GetRentedApartmentsByLandlord(int id);

        Task<Landlord> GetLandLordInfo(int id);
    }
}