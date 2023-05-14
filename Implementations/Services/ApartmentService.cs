using System;
using ApartmentRentManagementSystem.Dtos;
using ApartmentRentManagementSystem.Entities;
using System.Threading.Tasks;
using System.IO;
using ApartmentRentManagementSystem.Email;
using ApartmentRentManagementSystem.Interfaces.Repositories;

namespace ApartmentRentManagementSystem.Interfaces.Services
{
    public class ApartmentService : IApartmentService
    {
             
        private readonly IApartmentRepository _apartmentRepository;
        private readonly IImageRepository _imageRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IHouseEquipmentRepository _houseEquipmentRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUtilityRepository _utilityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _email;
        private readonly ILandlordRepository _landlordRepository;
        private readonly IWebHostEnvironment _webpost;
        public ApartmentService(IImageRepository imageRepository, IEmailSender email, IWebHostEnvironment webpost, ILandlordRepository landlordRepository, IUserRepository userRepository, IUtilityRepository utilityRepository, ICustomerRepository customerRepository, IHouseEquipmentRepository houseEquipmentRepository, IApartmentRepository apartmentRepository, ICategoryRepository categoryRepository, IAddressRepository addressRepository)
        {
            _email  = email;
            _apartmentRepository = apartmentRepository;
            _imageRepository = imageRepository;
            _addressRepository = addressRepository;
            _categoryRepository = categoryRepository;
            _houseEquipmentRepository = houseEquipmentRepository;
            _utilityRepository = utilityRepository;
            _customerRepository = customerRepository;
            _userRepository = userRepository;
            _landlordRepository = landlordRepository;
            _webpost = webpost;
        }

        public async Task<ApartmentResponseModel> RegisterApartment(ApartmentRequestModel model)
        {
            var user = await _userRepository.Get(x => x.Id == model.LandlordId);
            var landlord = await _landlordRepository.Get(x => x.UserId == user.Id);
            Console.WriteLine($"Service Line 31, {landlord.Id}");
            if (model == null)
            {
                return new ApartmentResponseModel
                {
                    Message = "Value can't be null",
                    Status = true
                };
            }

            var address = new Address
            {
                Country = model.Country,
                State = model.State,
                LGA = model.LGA,
                Description = model.AddressDescription
            };
            var addAddress = await _addressRepository.Register(address);
            var TermsAndCond = "";
           if (model.TermsAndCondition != null)
           {
               var path = _webpost.WebRootPath;
               var imagepath = Path.Combine(path, "Images");
               Directory.CreateDirectory(imagepath);
               var imagetype = model.TermsAndCondition.ContentType.Split('/')[1];
               TermsAndCond = $"{Guid.NewGuid()}.{imagetype}";
               var fullpath = Path.Combine(imagepath, TermsAndCond);
               using (var stream = new FileStream(fullpath, FileMode.Create))
               {
                    model.TermsAndCondition.CopyTo(stream);
               }
           }

            var apart = new Apartment
            {
                IsRented = false,
                AddressId = addAddress.Id,
                DamagesFee = model.DamagesFee,
                DamagesBalance = model.DamagesFee,
                TermsAndCondition = TermsAndCond,
                ApartmentTotalPrice = model.Price + model.DamagesFee,
                Price = model.Price,
                IsApproved = false,
                LandlordId = landlord.Id
            };
             var addApartment = await _apartmentRepository.Register(apart);
            
            foreach (var img in model.Images)
            {
                       var ig = new Image
                        {
                            Path = img,
                            ApartmentId = addApartment.Id
                        };
                    await _imageRepository.Register(ig);
            
             }
            var apartmentDto = new ApartmentDto
            {
                Id = addApartment.Id,
                AddressDescription = addApartment.Address.Description,
                State = addApartment.Address.State,
                LGA = addApartment.Address.LGA,
                Country = addApartment.Address.Country,
                ApartmentNumber = addApartment.ApartmentNumber,
                
            };
            var emailRequest = new EmailRequestModel{
                ReceiverName = landlord.User.UserName,
                ReceiverEmail = landlord.User.Email,
                Message = "You have successfully registered your apartment for rent",
                Subject = "Apartment Registration"
            };
            await _email.SendEmail(emailRequest);
            return new ApartmentResponseModel
            {
                Data = apartmentDto,
                Message = "An apartment Successfully Created",
                Status = true
            };
            
        }

        public async Task<BaseResponse> AddCategoryToApartment(ApartmentCategory model)
        {
             var apartment = await _apartmentRepository.GetApartmentInfo(model.ApartmentId);
            if (apartment == null)
            {
                    return new BaseResponse
                    {
                        Status = false,
                        Message = "Apartment not found"
                    };
            }

            var category = await _categoryRepository.Get(x => x.Name == model.Category);
            if (category == null)
            {
                return new BaseResponse
                {
                    Status = false,
                    Message = "Category not found"
                };
            }
            apartment.CategoryId = category.Id;
           var updateApartment = await _apartmentRepository.Update(apartment);
           if (updateApartment == null)
           {
               return new BaseResponse
                {
                    Status = false,
                    Message = "Not successfull"
                };
           }
            return new BaseResponse
            {
                Status = true,
                Message = "Category successfully Added to Apartment"
            };
        }
        public async Task<BaseResponse> UpdateApartment(UpdateApartmentRequestModel model, int id)
        {
             var apartment = await _apartmentRepository.Get(x => x.Id == id);
            if (apartment == null)
            {
                    return new BaseResponse
                    {
                        Status = false,
                        Message = "Apartment not found"
                    };
            }
            if (apartment.IsRented == true)
            {
                return new BaseResponse
                {
                    Status = false,
                    Message = "You can't update a rented apartment"
                };
            }
             var TermsAndCond = "";
           if (model.TermsAndCondition != null)
           {
               var path = _webpost.WebRootPath;
               var imagepath = Path.Combine(path, "Images");
               Directory.CreateDirectory(imagepath);
               var imagetype = model.TermsAndCondition.ContentType.Split('/')[1];
               TermsAndCond = $"{Guid.NewGuid()}.{imagetype}";
               var fullpath = Path.Combine(imagepath, TermsAndCond);
               using (var stream = new FileStream(fullpath, FileMode.Create))
               {
                    model.TermsAndCondition.CopyTo(stream);
               }
           }
            if (model.Images != null)
            {
                  var images = await _imageRepository.GetByExpression(i => i.ApartmentId == apartment.Id);
            if (images != null)
            {
                foreach (var item in images)
                {
                   await _imageRepository.Delete(item);
                }
            }
            
                foreach (var img in model.Images)
                {
                       var ig = new Image
                        {
                            Path = img,
                            ApartmentId = apartment.Id
                        };
                        await _imageRepository.Register(ig);
                }
            
            }
            
            apartment.TermsAndCondition = TermsAndCond ?? apartment.TermsAndCondition;
            apartment.Price = model.Price ?? apartment.Price;
            await _apartmentRepository.Update(apartment);
            return new BaseResponse
            {
                Status = true,
                Message = "Apartment successfully update"
            };

        }

        public async Task<ApartmentResponseModel> GetApartmentById(int id)
        {
            var apartment = await _apartmentRepository.GetApartmentInfo(id);
            if (apartment == null)
            {
                return new ApartmentResponseModel
                {
                    Message = "Apartment not found",
                    Status = false
                };
            } 
            Console.WriteLine((_imageRepository.GetImagesByApartmentId(apartment.Id)).Count);
            var apartmentDto = new ApartmentDto
            {
                Id = apartment.Id,
                AddressDescription = apartment.Address.Description,
                State = apartment.Address.State,
                LGA = apartment.Address.LGA,
                Country = apartment.Address.Country,
                Period = apartment.Period,
                TermsAndCondition = apartment.TermsAndCondition,
                ApartmentTotalPrice = apartment.ApartmentTotalPrice,
                Price = apartment.Price,
                LandlordId = apartment.LandlordId,
                ApartmentNumber = apartment.ApartmentNumber,
                Images = _imageRepository.GetImagesByApartmentId(apartment.Id),
                // Category = apartment.Category.Name,
                Utilities = _utilityRepository.GetUtilitysByApartmentId(apartment.Id),
                HouseEquipments = apartment.HouseEquipments.Select(x => new HouseEquipmentDto
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList()
            };
            return new ApartmentResponseModel
            {
                Data = apartmentDto,
                Message = "An apartment",
                Status = true
            };
        }

        public async Task<BaseResponse> RentApartment(int id, int customerId)
        {
             var apartment = await _apartmentRepository.GetApartmentInfo(id);
             var customer = await _customerRepository.GetCustomerInfo(customerId);
            if (apartment == null || customer == null)
            {
                return new BaseResponse
                {
                    Message = "Apartment not found",
                    Status = false
                };
            }
            if(customer.IsVerified == false)
            {
                return new BaseResponse
                {
                    Message = "You must be verified before renting an apartment",
                    Status = false
                };
            }
            if(apartment.IsApproved == true && apartment.IsRented == false)
            {
                apartment.IsRented = true;
                apartment.RentBy = customer.Id;
                await _apartmentRepository.Update(apartment);
                return new BaseResponse
                {
                    Status = true,
                    Message = "Apartment rent"
                };
            }
            return new BaseResponse
            {
                Status = false,
                Message = "Apartment is not available for rent"
            };
            
        }

        public async Task<ApartmentsResponseModel> GetRentedApartments()
        {
            var apartments = await _apartmentRepository.GetRentedApartments();
            System.Console.WriteLine(apartments.Count);
            var apartmentDto = apartments.Select(apartment => new ApartmentDto
            {
                Id = apartment.Id,
                AddressDescription = apartment.Address.Description,
                State = apartment.Address.State,
                LGA = apartment.Address.LGA,
                Country = apartment.Address.Country,
                Period = apartment.Period,
                TermsAndCondition = apartment.TermsAndCondition,
                Price = apartment.Price,
                ApartmentTotalPrice = apartment.ApartmentTotalPrice,
                ApartmentNumber = apartment.ApartmentNumber,
                LandlordId = apartment.LandlordId,
                Images =  _imageRepository.GetImagesByApartmentId(apartment.Id),
                Category = apartment.Category.Name,
                Utilities = _utilityRepository.GetUtilitysByApartmentId(apartment.Id),
                HouseEquipments = apartment.HouseEquipments.Where(x => x.IsDeleted == false).Select(x => new HouseEquipmentDto
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

        public async Task<BaseResponse> UnRentApartment(int id)
        {
             var apartment = await _apartmentRepository.GetApartmentInfo(id);
            if (apartment == null)
            {
                return new BaseResponse
                {
                    Message = "Apartment not found",
                    Status = false
                };
            }
            apartment.IsRented = false;
            apartment.RentBy = 0;
            await _apartmentRepository.Update(apartment);
            return new BaseResponse
            {
                Status = true,
                Message = "Apartment not available for rent"
            };
        }

        public async Task<ApartmentsResponseModel> GetUnRentedApartments()
        {
            var apartments = await _apartmentRepository.GetUnRentedApartments();
            var apartmentDto = apartments.Select(apartment => new ApartmentDto
            {
                Id = apartment.Id,
                AddressDescription = apartment.Address.Description,
                State = apartment.Address.State,
                LGA = apartment.Address.LGA,
                Country = apartment.Address.Country,
                Period = apartment.Period,
                TermsAndCondition = apartment.TermsAndCondition,
                Price = apartment.Price,
                ApartmentTotalPrice = apartment.ApartmentTotalPrice,
                LandlordId = apartment.LandlordId,
                ApartmentNumber = apartment.ApartmentNumber,
                Images = _imageRepository.GetImagesByApartmentId(apartment.Id),
                Utilities = _utilityRepository.GetUtilitysByApartmentId(apartment.Id),
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
                Message = "All apartments available for rent",
                Status = true
            };
        }

        public async Task<BaseResponse> ApproveApartment(int id)
        {
             var apartment = await _apartmentRepository.GetApartmentInfo(id);
            if (apartment == null )
            {
                return new BaseResponse
                {
                    Message = "Apartment not found",
                    Status = false
                };
            } 
            if (apartment.CategoryId == null)
            {
                return new BaseResponse
                {
                    Message = "No category is choosen for this apartment",
                    Status = false
                };
            }
            apartment.IsApproved = true;
            await _apartmentRepository.Update(apartment);
            return new BaseResponse
            {
                Status = true,
                Message = "Apartment approved"
            };
        }

        public async Task<ApartmentsResponseModel> GetApprovedApartments()
        {
            var apartments = await _apartmentRepository.GetApprovedApartments();
            var apartmentDto = apartments.Select(apartment => new ApartmentDto
            {
                Id = apartment.Id,
                AddressDescription = apartment.Address.Description,
                State = apartment.Address.State,
                LGA = apartment.Address.LGA,
                Country = apartment.Address.Country,
                Period = apartment.Period,
                TermsAndCondition = apartment.TermsAndCondition,
                Price = apartment.Price,
                ApartmentNumber = apartment.ApartmentNumber,
                ApartmentTotalPrice = apartment.ApartmentTotalPrice,
                LandlordId = apartment.LandlordId,
                Images = _imageRepository.GetImagesByApartmentId(apartment.Id),
                Utilities = _utilityRepository.GetUtilitysByApartmentId(apartment.Id),
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
                Message = "All apartments available for rent",
                Status = true
            };
        }

        
        public async Task<BaseResponse> DisApproveApartment(int id)
        {
             var apartment = await _apartmentRepository.GetApartmentInfo(id);
            if (apartment == null)
            {
                return new BaseResponse
                {
                    Message = "Apartment not found",
                    Status = false
                };
            } 
            apartment.IsApproved = false;
            await _apartmentRepository.Update(apartment);
            return new BaseResponse
            {
                Status = true,
                Message = "Apartment disapproved"
            };
        }

        public async Task<ApartmentsResponseModel> GetUnApprovedApartments()
        {
            var apartments = await _apartmentRepository.GetUnApprovedApartments();
            var apartmentDto = apartments.Select(apartment => new ApartmentDto
            {
                Id = apartment.Id,
                AddressDescription = apartment.Address.Description,
                State = apartment.Address.State,
                LGA = apartment.Address.LGA,
                Country = apartment.Address.Country,
                Period = apartment.Period,
                TermsAndCondition = apartment.TermsAndCondition,
                Price = apartment.Price,
                ApartmentNumber = apartment.ApartmentNumber,
                ApartmentTotalPrice = apartment.ApartmentTotalPrice,
                LandlordId = apartment.LandlordId,
                Images = _imageRepository.GetImagesByApartmentId(apartment.Id),
                Utilities = _utilityRepository.GetUtilitysByApartmentId(apartment.Id),
                HouseEquipments = apartment.HouseEquipments.Select(x => new HouseEquipmentDto
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList()
            }).ToList();

            return new ApartmentsResponseModel
            {
                Data = apartmentDto,
                Message = "All apartments available for rent",
                Status = true
            };
        }

        public async Task<ApartmentsResponseModel> GetApartmentsByCountry(string country)
        {
            var apartments = await _apartmentRepository.GetApartmentsByCountry(country);
            if(apartments == null)
            {
                return new ApartmentsResponseModel
                {
                    Message = "Apartments not available apartments in this country",
                    Status = false
                };
            }
            var apartmentDto = apartments.Select(apartment => new ApartmentDto
            {
                Id = apartment.Id,
                AddressDescription = apartment.Address.Description,
                State = apartment.Address.State,
                LGA = apartment.Address.LGA,
                Country = apartment.Address.Country,
                Period = apartment.Period,
                TermsAndCondition = apartment.TermsAndCondition,
                Price = apartment.Price,
                ApartmentNumber = apartment.ApartmentNumber,
                LandlordId = apartment.LandlordId,
                ApartmentTotalPrice = apartment.ApartmentTotalPrice,
                Images = _imageRepository.GetImagesByApartmentId(apartment.Id),
                Utilities = _utilityRepository.GetUtilitysByApartmentId(apartment.Id),
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
                Message = "All apartments available for rent",
                Status = true
            };
        }

        public async Task<ApartmentsResponseModel> GetApartmentsByState(string state)
        {
            var apartments = await _apartmentRepository.GetApartmentsByState(state);
            if(apartments == null)
            {
                return new ApartmentsResponseModel
                {
                    Message = "Apartments not available apartments in this state",
                    Status = false
                };
            }
            var apartmentDto = apartments.Select(apartment => new ApartmentDto
            {
                Id = apartment.Id,
                AddressDescription = apartment.Address.Description,
                State = apartment.Address.State,
                LGA = apartment.Address.LGA,
                Country = apartment.Address.Country,
                Period = apartment.Period,
                TermsAndCondition = apartment.TermsAndCondition,
                Price = apartment.Price,
                ApartmentNumber = apartment.ApartmentNumber,
                LandlordId = apartment.LandlordId,
                ApartmentTotalPrice = apartment.ApartmentTotalPrice,
                Utilities = _utilityRepository.GetUtilitysByApartmentId(apartment.Id),
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
                Message = "All apartments available for rent",
                Status = true
            };
        }

        public async Task<ApartmentsResponseModel> GetApartmentsByLGA(string Lga)
        {
            var apartments = await _apartmentRepository.GetApartmentsByLGA(Lga);
            if (apartments == null)
            {
                return new ApartmentsResponseModel
                {
                    Message = "Apartments not available apartments in this Local Goverment", 
                    Status = false
                };
            }
            var apartmentDto = apartments.Select(apartment => new ApartmentDto
            {
                Id = apartment.Id,
                AddressDescription = apartment.Address.Description,
                State = apartment.Address.State,
                LGA = apartment.Address.LGA,
                Country = apartment.Address.Country,
                Period = apartment.Period,
                TermsAndCondition = apartment.TermsAndCondition,
                Price = apartment.Price,
                ApartmentNumber = apartment.ApartmentNumber,
                ApartmentTotalPrice = apartment.ApartmentTotalPrice,
                LandlordId = apartment.LandlordId,
                Images = _imageRepository.GetImagesByApartmentId(apartment.Id),
                Utilities = _utilityRepository.GetUtilitysByApartmentId(apartment.Id),
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
                Message = "All apartments available for rent",
                Status = true
            };
        }
        
        public async Task<ApartmentsResponseModel> GetAllApartments()
        {
            var apartments = await _apartmentRepository.GetAllApartments();
            if (apartments == null)
            {
                return new ApartmentsResponseModel
                {
                    Message = "Apartments not available apartments in this Local Goverment", 
                    Status = false
                };
            }
            var apartmentDto = apartments.Select(apartment => new ApartmentDto
            {
                Id = apartment.Id,
                AddressDescription = apartment.Address.Description,
                State = apartment.Address.State,
                LGA = apartment.Address.LGA,
                Country = apartment.Address.Country,
                Period = apartment.Period,
                TermsAndCondition = apartment.TermsAndCondition,
                Price = apartment.Price,
                ApartmentNumber = apartment.ApartmentNumber,
                ApartmentTotalPrice = apartment.ApartmentTotalPrice,
                LandlordId = apartment.LandlordId,
                Images = _imageRepository.GetImagesByApartmentId(apartment.Id),
                Utilities = _utilityRepository.GetUtilitysByApartmentId(apartment.Id),
                // Category = apartment.Category.Name,
                HouseEquipments = apartment.HouseEquipments.Select(x => new HouseEquipmentDto
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList()
            }).ToList();

            return new ApartmentsResponseModel
            {
                Data = apartmentDto,
                Message = "All apartments available for rent",
                Status = true
            };
        }
     
    }
}  