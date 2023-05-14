using System;
using System;
using ApartmentRentManagementSystem.Dtos;
using ApartmentRentManagementSystem.Interfaces.Repositories;
using ApartmentRentManagementSystem.Entities;
using ApartmentRentManagementSystem.Interfaces.Services;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace ApartmentRentManagementSystem.Implementations.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }
        public async Task<UserResponseModel> Login(UserRequestModel model)
        {
            var user = await _userRepository.Get(x => x.Email == model.Email);
            
            if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                var roles = _roleRepository.GetRolesByUserId(user.Id);
                return new UserResponseModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles.Select(x => new RoleDto
                    {
                        Name = x.Name,
                        Description = x.Description
                    }).ToList(),
                    Message = "Successfully logged in",
                    Status = true
                };
            }
                return new UserResponseModel
                {
                    Message = "Invalid email or password",
                    Status = false
                };
        }
    }
}