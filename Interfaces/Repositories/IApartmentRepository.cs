using ApartmentRentManagementSystem.Entities;
namespace ApartmentRentManagementSystem.Interfaces.Repositories
{
    public interface IApartmentRepository : IRepository<Apartment>
    {
        
        Task<Apartment> GetApartmentInfo(int id);

        Task<IList<Apartment>> GetRentedApartments();

        Task<IList<Apartment>> GetUnRentedApartments();

        Task<IList<Apartment>> GetUnApprovedApartments();

        Task<IList<Apartment>> GetApprovedApartments();

        Task<IList<Apartment>> GetApartmentsByState(string state);
   
        Task<IList<Apartment>> GetApartmentsByCountry(string country);

        Task<IList<Apartment>> GetApartmentsByLGA(string lga);

        Task<IList<Apartment>> GetAllApartments();
       
    }
}