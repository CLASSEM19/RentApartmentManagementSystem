using System;
using System.IO;
using ApartmentRentManagementSystem.Dtos;
using ApartmentRentManagementSystem.Interfaces.Repositories;
using ApartmentRentManagementSystem.Entities;
using ApartmentRentManagementSystem.Identity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using ApartmentRentManagementSystem.Interfaces.Services;
using ApartmentRentManagementSystem.Email;


namespace ApartmentRentManagementSystem.Implementations.Services
{
    public class LandlordService : ILandlordService
    {
        private readonly IEmailSender _email;
        private readonly ILandlordRepository _landlordRepository;
        private readonly IUserRepository _userRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IWebHostEnvironment _webHost;
        private readonly ICategoryRepository _categoryRepository;
        public LandlordService(IWebHostEnvironment webHost, IEmailSender email, ICategoryRepository categoryRepository, IImageRepository imageRepository, ILandlordRepository landlordRepository, IRoleRepository roleRepository, IAddressRepository addressRepository, IUserRepository userRepository)
        {
            _email  = email;
            _landlordRepository = landlordRepository;
            _userRepository  = userRepository;
            _addressRepository  = addressRepository;
            _roleRepository  = roleRepository;
            _webHost  = webHost;
            _imageRepository  = imageRepository;
            _categoryRepository  = categoryRepository;
        }
        public async Task<BaseResponse> RegisterLandlord(LandlordRequestModel model)
        {
            var land = await _userRepository.Get(a => a.Email == model.Email);
            if (land != null)
            {
                return new BaseResponse
                {
                    Message = "Email or password already exist",
                    Status = false,
                };
            }
               var user = new User
                {
                     UserName = $"{model.FirstName} {model.LastName}",
                     Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                     Email = model.Email
                };
                Console.WriteLine($"{user.Password}");
                
                var addUser = await _userRepository.Register(user);
                var role = await _roleRepository.Get(x => x.Name == "Landlord");
                if (role == null)
                {
                    return new BaseResponse
                    {
                        Message = "Role not found",
                        Status = false
                    };
                }
                var userRole = new UserRole
                {
                    UserId = addUser.Id,
                    RoleId = role.Id
                };
                user.UserRoles.Add(userRole);
                var updateUser = await _userRepository.Update(user);
                
                var address = new Address
                {
                    Country = model.Country,
                    State = model.State,
                    LGA = model.LGA,
                    Description = model.AddressDescription
                };
                var addAddress = await _addressRepository.Register(address);
                var  landlord = new Landlord
                {
                     FirstName = model.FirstName,
                     LastName = model.LastName,
                     PhoneNumber = model.PhoneNumber,
                     UserId = addUser.Id,
                     AddressId = addAddress.Id,
                };
                var addLandlord = await _landlordRepository.Register(landlord);
                
                    landlord.CreatedBy = addLandlord.Id;
                    landlord.LastModifiedBy = addLandlord.Id;
                    landlord.IsDeleted = false;
                await _landlordRepository.Update(addLandlord);

                var emailRequest = new EmailRequestModel{
                ReceiverName = landlord.User.UserName,
                ReceiverEmail = landlord.User.Email,
                Message = "You have successfully registered as a Landlord on Apartment Rent Mangement System, You can give ur apartment out for rent",
                Subject = "Landlord Registration"
            };
            await _email.SendEmail(emailRequest);
                return new BaseResponse
                {
                    Message = "Landlord Successfully registered",
                    Status = true,
                };
           
        }
        public async Task<BaseResponse> UpdateLandlord(UpdateLandlordRequestModel model, int id)
        {
              var land = await _landlordRepository.GetLandLordInfo(id);
            if (land == null)
            {
                return new BaseResponse
                {
                    Message = "Lanlord not found",
                    Status = false,
                };
            }
    
           land.User.UserName = $"{model.FirstName} {model.LastName}";
           land.User.Email = model.Email ?? land.User.Email;    
           land.Address.Country = model.Country ?? land.Address.Country;
           land.Address.State = model.State ?? land.Address.State;
           land.Address.LGA = model.LGA ?? land.Address.LGA;
           land.Address.Description = model.AddressDescription ?? land.Address.Description;
           land.FirstName = model.FirstName ?? land.FirstName;
           land.LastName = model.LastName ?? land.LastName;
           land.PhoneNumber = model.PhoneNumber ?? land.PhoneNumber;
           land.BankCode = model.BankName ?? land.BankCode;
           land.AccountNumber = model.AccountNumber ?? land.AccountNumber;
           land.Image = model.Image ?? land.Image;
            await _landlordRepository.Update(land);
            return new BaseResponse
            {
                Message = "Profile Successfully updated",
                Status = true,
            };
           
        }

        public async Task<LandlordResponseModel> GetLandLordByEmail(string email)
        {
            var landlord = await _landlordRepository.GetLandLordWithUser(email);
            if (landlord == null)
            {
                return new LandlordResponseModel
                {
                    Message = "Landlord not found",
                    Status = false
                };
            } 
            var landlordDto = new LandlordDto
            {
                
                Id = landlord.Id,
                Image = landlord.Image,
                FullName = $"{landlord.FirstName} {landlord.LastName}",
                PhoneNumber = landlord.PhoneNumber,
                Email = landlord.User.Email,
                AddressDescription = landlord.Address.Description,
                State = landlord.Address.State,
                LGA = landlord.Address.LGA,
                Country = landlord.Address.Country,
            };
            return new LandlordResponseModel
            {
                Message = "Landlord Successfully retrieved",
                Status = true,
                Data = landlordDto
            };
        }
        public async Task<LandlordResponseModel> GetLandLordById(int id)
        {
            
            var landlord = await _landlordRepository.GetLandLordWithUser(id);
            if (landlord == null)
            {
                return new LandlordResponseModel
                {
                    Message = "Landlord not found",
                    Status = false
                };
            } 
            var landlordDto = new LandlordDto
            {
                Id = landlord.Id,
                Image = landlord.Image,
                BankName = landlord.BankCode,
                AccountNumber = landlord.AccountNumber,
                FullName = $"{landlord.FirstName} {landlord.LastName}",
                PhoneNumber = landlord.PhoneNumber,
                Email = landlord.User.Email,
                AddressDescription = landlord.Address.Description,
                State = landlord.Address.State,
                LGA = landlord.Address.LGA,
                Country = landlord.Address.Country,
            };
            return new LandlordResponseModel
            {
                Message = "Landlord Successfully retrieved",
                Status = true,
                Data = landlordDto
            };
        }

        public async Task<LandlordResponseModel> GetLandLordInfo(int id)
        {
            
            var landlord = await _landlordRepository.GetLandLordInfo(id);
            if (landlord == null)
            {
                return new LandlordResponseModel
                {
                    Message = "Landlord not found",
                    Status = false
                };
            } 
            var landlordDto = new LandlordDto
            {
                Id = landlord.Id,
                Image = landlord.Image,
                BankName = landlord.BankCode,
                AccountNumber = landlord.AccountNumber,
                FullName = $"{landlord.FirstName} {landlord.LastName}",
                PhoneNumber = landlord.PhoneNumber,
                Email = landlord.User.Email,
                AddressDescription = landlord.Address.Description,
                State = landlord.Address.State,
                LGA = landlord.Address.LGA,
                Country = landlord.Address.Country
            };
            return new LandlordResponseModel
            {
                Message = "Landlord Successfully retrieved",
                Status = true,
                Data = landlordDto
            };
        }

        public async Task<BaseResponse> DeActivateLandlord(int id)
        {

            var landlord = await _landlordRepository.Get(x => x.Id == id);
            if (landlord == null)
            {
                return new LandlordResponseModel
                {
                    Message = "Landlord not found",
                    Status = false
                };
            }
            landlord.IsDeleted = false;
            await  _landlordRepository.Update(landlord);
            return new LandlordResponseModel
            {
                Message = "Landlord Successfully deactivate",
                Status = true
            };
        }

        public async Task<BaseResponse> ActivateLandlord(int id)
        {
            var landlord = await _landlordRepository.Get(x => x.Id == id);
            if (landlord == null)
            {
                return new LandlordResponseModel
                {
                    Message = "Landlord not found",
                    Status = false
                };
            }
            landlord.IsDeleted = true;
            await  _landlordRepository.Update(landlord);
            return new LandlordResponseModel
            {
                Message = "Landlord Successfully activate",
                Status = true
            };
        }

        public async Task<LandlordsResponseModel> GetAllActivatedLandlords()
        {
             var landlords = await _landlordRepository.GetAllActivatedLandlord();
           var landlordDto = landlords.Select(x => new LandlordDto
           {
                Id = x.Id,
                Image = x.Image,
                FullName = $"{x.FirstName} {x.LastName}",
                PhoneNumber = x.PhoneNumber,
                Email = x.User.Email,
                AddressDescription = x.Address.Description,
                State = x.Address.State,
                LGA = x.Address.LGA,
                Country = x.Address.Country
           }).ToList();

           return new LandlordsResponseModel
           {
               Data = landlordDto,
               Message = "List of all Landlords",
                Status = true
           };

        }

        public async Task<LandlordsResponseModel> GetAllDeactivatedLandlords()
        {
             var landlords = await _landlordRepository.GetAllDeactivatedLandlord();
           var landlordDto = landlords.Select(x => new LandlordDto
           {
                Id = x.Id,
                Image = x.Image,
                FullName = $"{x.FirstName} {x.LastName}",
                PhoneNumber = x.PhoneNumber,
                Email = x.User.Email,
                AddressDescription = x.Address.Description,
                State = x.Address.State,
                LGA = x.Address.LGA,
                Country = x.Address.Country
           }).ToList();

           return new LandlordsResponseModel
           {
               Data = landlordDto,
               Message = "List of all Landlords",
                Status = true
           };

        }

        public async Task<LandlordsResponseModel> GetAllLandlords()
        {
            var landlords = await _landlordRepository.GetAllLandLordWithUser();
           var landlordDto = landlords.Select(x => new LandlordDto
           {
                Id = x.Id,
                Image = x.Image,
                FullName = $"{x.FirstName} {x.LastName}",
                PhoneNumber = x.PhoneNumber,
                Email = x.User.Email,
                AddressDescription = x.Address.Description,
                State = x.Address.State,
                LGA = x.Address.LGA,
                Country = x.Address.Country
           }).ToList();

           return new LandlordsResponseModel
           {
               Data = landlordDto,
               Message = "List of all Landlords",
                Status = true
           };
        }

        public async Task<ApartmentsResponseModel> GetApartmentsByLandlord(int id)
        {
            var apart = await _landlordRepository.GetApartmentsByLandlord(id);
            if (apart == null)
            {
                return new ApartmentsResponseModel
                {
                    Message = "Landlord not found",
                    Status = false
                };
            }
            var apartmentDto = apart.Select(apartment => new ApartmentDto
            {
                Id = apartment.Id,
                IsRented = apartment.IsRented,
                IsApproved = apartment.IsApproved,
                Period = apartment.Period,
                TermsAndCondition = apartment.TermsAndCondition,
                Price = apartment.Price,
            }).ToList();

            return new ApartmentsResponseModel
            {
                Data = apartmentDto,
                Message = "All aprtments",
                Status = true
            };
        }

        public async Task<ApartmentsResponseModel> GetApartmentsByUserId(int id)
        {
            Console.WriteLine($"service Landlordid= {id}");
            var landlord = await _landlordRepository.Get(x => x.UserId == id);
            var apart = await _landlordRepository.GetApartmentsByLandlord(landlord.Id);
            if (apart == null)
            {
                return new ApartmentsResponseModel
                {
                    Message = "Landlord not found",
                    Status = false
                };
            }
            var apartmentDto = apart.Select(apartment => new ApartmentDto
            {
                Id = apartment.Id,
                IsRented = apartment.IsRented,
                IsApproved = apartment.IsApproved,
                Period = apartment.Period,
                TermsAndCondition = apartment.TermsAndCondition,
                Price = apartment.Price,
            }).ToList();

            return new ApartmentsResponseModel
            {
                Data = apartmentDto,
                Message = "All aprtments",
                Status = true
            };
        }


        public async Task<ApartmentsResponseModel> GetRentedApartmentsByLandlord(int id)
        {
            
            var landlord = await _landlordRepository.Get(x => x.Id == id);
            if (landlord == null)
            {
                return new ApartmentsResponseModel
                {
                    Message = "Landlord not found",
                    Status = true
                };
            }
            var apart = await _landlordRepository.GetRentedApartmentsByLandlord(id);
            if (apart == null)
            {
                return new ApartmentsResponseModel
                {
                    Message = "No Approved Apartments available",
                    Status = true
                };
            }
            var apartmentDto = apart.Select(apartment => new ApartmentDto
            {

                Address = $"{apartment.Address.Description}, {apartment.Address.LGA}, {apartment.Address.City}, {apartment.Address.State}, {apartment.Address.Country}.",
                Period = apartment.Period,
                TermsAndCondition = apartment.TermsAndCondition,
                Price = apartment.Price,
                LandlordId = apartment.LandlordId,
                Images = _imageRepository.GetImagesByApartmentId(apartment.Id),
                Category = apartment.Category.Name,
                HouseEquipments = apartment.HouseEquipments.Select(x => new HouseEquipmentDto
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList()
            }).ToList();

            return new ApartmentsResponseModel
            {
                Data = apartmentDto,
                Message = "All rented aprtments",
                Status = true
            };
        }

        public async Task<ApartmentsResponseModel> GetUnRentedApartmentsByLandlord(int id)
        {
            var landlord = await _landlordRepository.Get(x => x.Id == id);
            if (landlord == null)
            {
                return new ApartmentsResponseModel
                {
                    Message = "Landlord not found",
                    Status = true
                };
            }
            var apart = await _landlordRepository.GetUnRentedApartmentsByLandlord(id);
            if (apart == null)
            {
                return new ApartmentsResponseModel
                {
                    Message = "No Approved Apartments available",
                    Status = true
                };
            }
            var apartmentDto = apart.Select(apartment => new ApartmentDto
            {

                Address = $"{apartment.Address.Description}, {apartment.Address.LGA}, {apartment.Address.City}, {apartment.Address.State}, {apartment.Address.Country}.",
                Period = apartment.Period,
                TermsAndCondition = apartment.TermsAndCondition,
                Price = apartment.Price,
                LandlordId = apartment.LandlordId,
                Images = _imageRepository.GetImagesByApartmentId(apartment.Id),
                Category = apartment.Category.Name,
                HouseEquipments = apartment.HouseEquipments.Select(x => new HouseEquipmentDto
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList()
            }).ToList();

            return new ApartmentsResponseModel
            {
                Data = apartmentDto,
                Message = "All rented aprtments",
                Status = true
            };
        }

        public async Task<ApartmentsResponseModel> GetUnApprovedApartmentsByLandlord(int id)
        {
            var landlord = await _landlordRepository.Get(x => x.Id == id);
            if (landlord == null)
            {
                return new ApartmentsResponseModel
                {
                    Message = "Landlord not found",
                    Status = true
                };
            }
            var apart = await _landlordRepository.GetUnApprovedApartmentsByLandlord(id);
            if (apart == null)
            {
                return new ApartmentsResponseModel
                {
                    Message = "No Approved Apartments available",
                    Status = true
                };
            }
            var apartmentDto = apart.Select(apartment => new ApartmentDto
            {

                Address = $"{apartment.Address.Description}, {apartment.Address.LGA}, {apartment.Address.City}, {apartment.Address.State}, {apartment.Address.Country}.",
                Period = apartment.Period,
                TermsAndCondition = apartment.TermsAndCondition,
                Price = apartment.Price,
                LandlordId = apartment.LandlordId,
                Images = _imageRepository.GetImagesByApartmentId(apartment.Id),
                Category = apartment.Category.Name,
                HouseEquipments = apartment.HouseEquipments.Select(x => new HouseEquipmentDto
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList()
            }).ToList();

            return new ApartmentsResponseModel
            {
                Data = apartmentDto,
                Message = "All rented aprtments",
                Status = true
            };
        }


        public async Task<ApartmentsResponseModel> GetApprovedApartmentsByLandlord(int id)
        {
            var landlord = await _landlordRepository.Get(x => x.Id == id);
            if (landlord == null)
            {
                return new ApartmentsResponseModel
                {
                    Message = "Landlord not found",
                    Status = true
                };
            }
            var apart = await _landlordRepository.GetApprovedApartmentsByLandlord(id);
            if (apart == null)
            {
                return new ApartmentsResponseModel
                {
                    Message = "No Approved Apartments available",
                    Status = true
                };
            }
            var apartmentDto = apart.Select(apartment => new ApartmentDto
            {

                Address = $"{apartment.Address.Description}, {apartment.Address.LGA}, {apartment.Address.City}, {apartment.Address.State}, {apartment.Address.Country}.",
                Period = apartment.Period,
                TermsAndCondition = apartment.TermsAndCondition,
                Price = apartment.Price,
                LandlordId = apartment.LandlordId,
                Images = _imageRepository.GetImagesByApartmentId(apartment.Id),
                Category = apartment.Category.Name,
                HouseEquipments = apartment.HouseEquipments.Select(x => new HouseEquipmentDto
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList()
            }).ToList();

            return new ApartmentsResponseModel
            {
                Data = apartmentDto,
                Message = "All rented aprtments",
                Status = true
            };
        }
    }
}
        
        