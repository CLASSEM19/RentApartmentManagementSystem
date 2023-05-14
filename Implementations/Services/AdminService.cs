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

namespace ApartmentRentManagementSystem.Implementations.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IUserRepository _userRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IWebHostEnvironment _webHost;
        private readonly ICategoryRepository _categoryRepository;
        public AdminService(IWebHostEnvironment webHost, ICategoryRepository categoryRepository, IImageRepository imageRepository, IAdminRepository adminRepository, IRoleRepository roleRepository, IAddressRepository addressRepository, IUserRepository userRepository)
        {
            _adminRepository = adminRepository;
            _userRepository  = userRepository;
            _addressRepository  = addressRepository;
            _roleRepository  = roleRepository;
            _webHost  = webHost;
            _imageRepository  = imageRepository;
            _categoryRepository  = categoryRepository;
        }
        public async Task<BaseResponse> RegisterAdmin(AdminRequestModel model)
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
                var role = await _roleRepository.Get(x => x.Name == "admin");
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
                var  admin = new Admin
                {
                     FirstName = model.FirstName,
                     LastName = model.LastName,
                     PhoneNumber = model.PhoneNumber,
                     UserId = addUser.Id,
                     AddressId = addAddress.Id
                };
                var addadmin = await _adminRepository.Register(admin);
                
                    admin.CreatedBy = addadmin.Id;
                    admin.LastModifiedBy = addadmin.Id;
                    admin.IsDeleted = false;
                      var folderPath = Path.Combine(Directory.GetCurrentDirectory() + "\\Images\\");
                if (!System.IO.Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                if(model.Image != null)
                {

                    var fileName = Path.GetFileNameWithoutExtension(model.Image.FileName);   
                    var filePath = Path.Combine(folderPath, model.Image.FileName);
                    var extension = Path.GetExtension(model.Image.FileName);
                    if (!System.IO.Directory.Exists(filePath))
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.Image.CopyToAsync(stream);
                        }
                         admin.Image = fileName;
                    }
                }
               await _adminRepository.Update(admin);
                return new BaseResponse
                {
                    Message = "admin Successfully registered",
                    Status = true,
                };
           return new BaseResponse
            {
                Message = "Value cannot be null ",
                Status = false,
            };
        }
        public async Task<BaseResponse> UpdateAdmin(UpdateAdminRequestModel model, int id)
        {
            if (model == null)
            {
                return new BaseResponse
                {
                    Message = "Value most not be null",
                    Status = false
                };
            }
                var admin = await _adminRepository.Get(x => x.Id == id);
                var address = await _addressRepository.Get(x => x.Id == admin.AddressId);
            if (admin == null || address == null)
            {
                return new BaseResponse
                {
                    Message = "admin not found",
                    Status = false
                };
            }
                var folderPath = Path.Combine(Directory.GetCurrentDirectory() + "\\Images\\");
                if (!System.IO.Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                    var fileName = Path.GetFileNameWithoutExtension(model.Image.FileName);   
                    var filePath = Path.Combine(folderPath, model.Image.FileName);
                    var extension = Path.GetExtension(model.Image.FileName);
                    if (!System.IO.Directory.Exists(filePath))
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.Image.CopyToAsync(stream);
                        }
                         admin.Image = fileName;
                    }
                 await _adminRepository.Update(admin);
                    address.Country = model.Country;
                    address.State = model.State;
                    address.LGA = model.LGA;
                    address.Description = model.AddressDescription;                
                var updateAddress = await _addressRepository.Update(address);
                
                     admin.PhoneNumber = model.PhoneNumber;
                var updateadmin = await _adminRepository.Update(admin);
                
               
                return new BaseResponse
                {
                    Message = "admin Succesfully Updated",
                    Status = true
                };
        }
        public async Task<AdminResponseModel> GetAdminInfo(int id)
        {
            var admin = await _adminRepository.GetAdminInfo(id);
            if (admin == null)
            {
                return new AdminResponseModel
                {
                    Message = "admin not found",
                    Status = false
                };
            } 
            var adminDto = new AdminDto
            {
                Id = admin.Id,
                Image = admin.Image,
                FullName = $"{admin.FirstName} {admin.LastName}",
                PhoneNumber = admin.PhoneNumber,
                Email = admin.User.Email,
                Address = $"{admin.Address.Description}, {admin.Address.LGA} LocalGovernment, {admin.Address.City}, {admin.Address.State} State, {admin.Address.Country}.",
            };
            return new AdminResponseModel
            {
                Message = "admin Successfully retrieved",
                Status = true,
                Data = adminDto
            };
        }

        public async Task<BaseResponse> DeActivateAdmin(int id)
        {
            var admin = await _adminRepository.Get(x => x.Id == id);
            if (admin == null)
            {
                return new AdminResponseModel
                {
                    Message = "admin not found",
                    Status = false
                };
            }
            admin.IsDeleted = false;
            await  _adminRepository.Update(admin);
            return new AdminResponseModel
            {
                Message = "admin Successfully deactivate",
                Status = true
            };
        }

        public async Task<BaseResponse> ActivateAdmin(int id)
        {
            var admin = await _adminRepository.Get(x => x.Id == id);
            if (admin == null)
            {
                return new BaseResponse
                {
                    Message = "admin not found",
                    Status = false
                };
            }
            admin.IsDeleted = true;
            await  _adminRepository.Update(admin);
            return new BaseResponse
            {
                Message = "admin Successfully activate",
                Status = true
            };
        }

        public async Task<AdminsResponseModel> GetAllActivatedAdmins()
        {
             var admins = await _adminRepository.GetAllActivatedAdmin();
           var adminDto = admins.Select(x => new AdminDto
           {
                Id = x.Id,
                Image = x.Image,
                FullName = $"{x.FirstName} {x.LastName}",
                PhoneNumber = x.PhoneNumber,
                Email = x.User.Email,
                Address = $"{x.Address.Description}, {x.Address.LGA} LocalGovernment, {x.Address.City}, {x.Address.State} State, {x.Address.Country}.",
           }).ToList();

           return new AdminsResponseModel
           {
               Data = adminDto,
               Message = "List of all admins",
                Status = true
           };

        }

        public async Task<AdminsResponseModel> GetAllDeactivatedAdmins()
        {
             var admins = await _adminRepository.GetAllDeactivatedAdmin();
           var adminDto = admins.Select(x => new AdminDto
           {
                Id = x.Id,
                Image = x.Image,
                FullName = $"{x.FirstName} {x.LastName}",
                PhoneNumber = x.PhoneNumber,
                Email = x.User.Email,
                Address = $"{x.Address.Description}, {x.Address.LGA} LocalGovernment, {x.Address.City}, {x.Address.State} State, {x.Address.Country}.",
           }).ToList();

           return new AdminsResponseModel
           {
               Data = adminDto,
               Message = "List of all admins",
                Status = true
           };

        }

        public async Task<AdminsResponseModel> GetAllAdmins()
        {
            var admins = await _adminRepository.GetAllAdminWithUser();
           var adminDto = admins.Select(x => new AdminDto
           {
                Id = x.Id,
                Image = x.Image,
                FullName = $"{x.FirstName} {x.LastName}",
                PhoneNumber = x.PhoneNumber,
                Email = x.User.Email,
                Address = $"{x.Address.Description}, {x.Address.LGA} LocalGovernment, {x.Address.City}, {x.Address.State} State, {x.Address.Country}.",
           }).ToList();

           return new AdminsResponseModel
           {
               Data = adminDto,
               Message = "List of all admins",
                Status = true
           };
        }
    }
}