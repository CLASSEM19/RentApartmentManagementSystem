using System;
using ApartmentRentManagementSystem.Dtos;
using ApartmentRentManagementSystem.Entities;
using ApartmentRentManagementSystem.Interfaces.Repositories;
using ApartmentRentManagementSystem.Interfaces.Services;

namespace ApartmentRentManagementSystem.Implementations.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly IUtilityRepository _utilityRepository;

        private readonly IApartmentRepository _apartmentRepository;

        public UtilityService(IUtilityRepository utilityRepository, IApartmentRepository apartmentRepository)
        {
            _utilityRepository = utilityRepository;
            _apartmentRepository = apartmentRepository;
        }


       public async Task<BaseResponse> AddUtility(UtilityRequestModel dto)
       {
           var apartment = await _apartmentRepository.Get(x => x.Id == dto.ApartmentId);
           if (apartment == null)
           {
               return new BaseResponse
               {
                   Message = "Apartment not found",
                   Status = false
               };
           }       
                    var utility = new Utility
                    {
                        Name = dto.Name,
                        Number = dto.Quantity,
                        Description = dto.Description,
                        ApartmentId = apartment.Id
                    };
                    await _utilityRepository.Register(utility);
           
            return new BaseResponse
            {
                Message = "Utility Successfully add",
                Status = true
            };
       }

        // public async Task<BaseResponse> DeleteEquipmentFromApartment(int id, string name)
        // {
        //     var Utility = await _UtilityRepository.Get(x => x.Name == name && x.ApartmentId == id);
        //     if (Utility == null)
        //     {
        //         return new BaseResponse
        //         {
        //             Message = "Equipment not found",
        //             Status = false
        //         };
        //     }
        //     Utility.IsDeleted = true;
        //     await _UtilityRepository.Update(Utility);

        //     return new BaseResponse
        //     {
        //         Message = "Equiment Successfully deleted",
        //         Status = true
        //     };
        // }
    }
}