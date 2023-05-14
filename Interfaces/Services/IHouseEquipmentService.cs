using System;
using ApartmentRentManagementSystem.Dtos;
using System.Threading.Tasks;
namespace ApartmentRentManagementSystem.Interfaces.Services
{
    public interface IHouseEquipmentService
    {
       Task<BaseResponse> AddNewEquipmentToApartment(HouseEquipmentRequestModel dto);

        Task<BaseResponse> DeleteEquipmentFromApartment(int id, string name);
       
    }
}