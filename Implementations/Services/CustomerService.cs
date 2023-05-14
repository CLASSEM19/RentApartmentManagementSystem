using System;
using ApartmentRentManagementSystem.Email;
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
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmailSender _email;
        private readonly IUserRepository _userRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IWebHostEnvironment _webHost;
        private readonly ICategoryRepository _categoryRepository;
        public CustomerService(IWebHostEnvironment webHost, IEmailSender email, ICategoryRepository categoryRepository, IImageRepository imageRepository, ICustomerRepository customerRepository, IRoleRepository roleRepository, IAddressRepository addressRepository, IUserRepository userRepository)
        {
            _customerRepository = customerRepository;
            _userRepository  = userRepository;
            _addressRepository  = addressRepository;
            _roleRepository  = roleRepository;
            _webHost  = webHost;
            _email  = email;
            _imageRepository  = imageRepository;
            _categoryRepository  = categoryRepository;
        }
        public async Task<BaseResponse> RegisterCustomer(CustomerRequestModel model)
        {
            
            var customer = await _userRepository.Get(a => a.Email == model.Email);
            if (customer != null)
            {
                return new BaseResponse
                {
                    Message = "Email already exist",
                    Status = false
                };
            }
            else if(model != null)
            {
                var user = new User
                {
                     UserName = $"{model.FirstName} {model.LastName}",
                     Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                     Email = model.Email
                };
                var addUser = await _userRepository.Register(user);
                var role = await _roleRepository.Get(x => x.Name == "Customer");
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
                var  Customer = new Customer
                {
                    
                     FirstName = model.FirstName,
                     LastName = model.LastName,
                     PhoneNumber = model.PhoneNumber,
                     UserId = addUser.Id,
                     AddressId = addAddress.Id,
                     Image = model.Image
                };
                    var addCustomer = await _customerRepository.Register(Customer);
                    addCustomer.CreatedBy = addCustomer.Id;
                    addCustomer.LastModifiedBy = addCustomer.Id;
                    addCustomer.IsDeleted = false;
               await _customerRepository.Update(addCustomer);
             var emailRequest = new EmailRequestModel{
                ReceiverName = addUser.UserName,
                ReceiverEmail = addUser.Email,
                Message = "You have successfully registered as a Customer on Apartment Rent Mangement System, We provide a peaceful abode, for your satisfaction.",
                Subject = "Customer Registration"
            };
            await _email.SendEmail(emailRequest);
                return new BaseResponse
                {
                    Message = "Customer Succesfully registered",
                    Status = true
                };

            }
            return new BaseResponse
            {
                Message = "Value most not be null",
                Status = false
            };
        }
        public async Task<BaseResponse> UpdateCustomer(UpdateCustomerRequestModel model, int id)
        {
            if (model == null)
            {
                return new BaseResponse
                {
                    Message = "Value most not be null",
                    Status = false
                };
            }
                var customer = await _customerRepository.GetCustomerInfo(id);
            if (customer == null)
            {
                return new BaseResponse
                {
                    Message = "Customer not found",
                    Status = false
                };
            }
           customer.User.UserName = $"{model.FirstName} {model.LastName}" ?? customer.User.UserName;
           customer.User.Email = model.Email ?? customer.User.Email;    
           customer.Address.Country = model.Country ?? customer.Address.Country;
           customer.Address.State = model.State ?? customer.Address.State;
           customer.Address.LGA = model.LGA ?? customer.Address.LGA;
           customer.Address.Description = model.AddressDescription ?? customer.Address.Description;
           customer.FirstName = model.FirstName ?? customer.FirstName;
           customer.LastName = model.LastName ?? customer.LastName;
           customer.PhoneNumber = model.PhoneNumber ?? customer.PhoneNumber;
           customer.Image = model.Image ?? customer.Image;
            await _customerRepository.Update(customer);
              
            return new BaseResponse
            {
                Message = "Customer Succesfully Updated",
                Status = true
            };
        }

        public async Task<CustomerResponseModel> GetCustomerInfo(int id)
        {
            var customer = await _customerRepository.GetCustomerInfo(id);
            if (customer == null)
            {
                return new CustomerResponseModel
                {
                    Message = "Customer not found",
                    Status = false
                };
            } 
            var customerDto = new CustomerDto
            {
                Id = customer.Id,
                Image = customer.Image,
                FullName = $"{customer.FirstName} {customer.LastName}",
                PhoneNumber = customer.PhoneNumber,
                Email = customer.User.Email,
                AddressDescription = customer.Address.Description,
                State = customer.Address.State,
                LGA = customer.Address.LGA,
                Country = customer.Address.Country
            };
            return new CustomerResponseModel
            {
                Message = "Customer Successfully retrieved",
                Status = true,
                Data = customerDto
            };
        }

        
        public async Task<CustomersResponseModel> GetAllCustomers()
        {
            var customers = await _customerRepository.GetAllCustomerWithUser();
           var customerDto = customers.Select(x => new CustomerDto
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

           return new CustomersResponseModel
           {
               Data = customerDto,
               Message = "List of all Customers",
                Status = true
           };
        }

        public async Task<ApartmentsResponseModel> GetApartmentsByCustomerId(int id)
        {
            var apart = await _customerRepository.GetApartmentsByCustomer(id);
            if (apart == null)
            {
                return new ApartmentsResponseModel
                {
                    Message = "Customer not found",
                    Status = true
                };
            }
            var apartmentDto = apart.Select(apartment => new ApartmentDto
            {

                AddressDescription = apartment.Address.Description,
                State = apartment.Address.State,
                LGA = apartment.Address.LGA,
                Country = apartment.Address.Country,
                Period = apartment.Period,
                TermsAndCondition = apartment.TermsAndCondition,
                Price = apartment.Price,
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
        public async Task<BaseResponse> VerifyCustomer(int id)
        {
            var customer = await _customerRepository.Get(c => c.Id == id);
            customer.IsVerified = true;
          var updatecustomer =   await _customerRepository.Update(customer);
          if (updatecustomer != null)
          {
                return new BaseResponse
                {
                    Message = "Customer Verified",
                    Status = true
                };
          }
            return new BaseResponse
            {
                Message = "Verification successful",
                Status = false
            };
        }
        public async Task<CustomersResponseModel> GetAllVerifiedCustomers()
        {
            var customers = await _customerRepository.GetAllVerifiedCustomers();
           var customerDto = customers.Select(x => new CustomerDto
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

           return new CustomersResponseModel
           {
               Data = customerDto,
               Message = "List of all Customers",
                Status = true
           };
        }

        public async Task<CustomersResponseModel> GetNotVerifiedCustomers()
        {
            var customers = await _customerRepository.GetNotVerifiedCustomers();
           var customerDto = customers.Select(x => new CustomerDto
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

           return new CustomersResponseModel
           {
               Data = customerDto,
               Message = "List of all Customers",
                Status = true
           };
        }
    }
}
        
        