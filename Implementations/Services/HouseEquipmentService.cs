using System;
using ApartmentRentManagementSystem.Dtos;
using ApartmentRentManagementSystem.Entities;
using ApartmentRentManagementSystem.Interfaces.Repositories;
using ApartmentRentManagementSystem.Interfaces.Services;

namespace ApartmentRentManagementSystem.Implementations.Services
{
    public class HouseEquipmentService : IHouseEquipmentService
    {
        private readonly IHouseEquipmentRepository _houseEquipmentRepository;

        private readonly IApartmentRepository _apartmentRepository;

        public HouseEquipmentService(IHouseEquipmentRepository houseEquipmentRepository, IApartmentRepository apartmentRepository)
        {
            _houseEquipmentRepository = houseEquipmentRepository;
            _apartmentRepository = apartmentRepository;
        }


       public async Task<BaseResponse> AddNewEquipmentToApartment(HouseEquipmentRequestModel dto)
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
           var houseEquipment = await _houseEquipmentRepository.Get(x => x.Name == dto.Name);
            if (houseEquipment == null)
            {                
                var houseEquip = new HouseEquipment
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    ApartmentId = apartment.Id
                };
                await _houseEquipmentRepository.Register(houseEquip);
            }
            else
            {
                houseEquipment.Name = dto.Name;
                houseEquipment.Description = dto.Description;
                houseEquipment.ApartmentId = apartment.Id;
                await _houseEquipmentRepository.Update(houseEquipment);
            }

            return new BaseResponse
            {
                Message = "Equipment Successfully Add",
                Status = true
            };
       }

        public async Task<BaseResponse> DeleteEquipmentFromApartment(int id, string name)
        {
            var houseEquipment = await _houseEquipmentRepository.Get(x => x.Name == name && x.ApartmentId == id);
            if (houseEquipment == null)
            {
                return new BaseResponse
                {
                    Message = "Equipment not found",
                    Status = false
                };
            }
            houseEquipment.IsDeleted = true;
            await _houseEquipmentRepository.Update(houseEquipment);

            return new BaseResponse
            {
                Message = "Equiment Successfully deleted",
                Status = true
            };
        }
    }
}