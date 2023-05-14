using ApartmentRentManagementSystem.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;

namespace ApartmentRentManagementSystem.Authentication
{
    public class JWTAuthentication : IJWTAuthentication
    {
        public string _key;
        public JWTAuthentication(string key)
        {
            _key = key;
        }
        public string GenerateToken(UserResponseModel model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);
             var claims = new List<Claim>();
             claims.Add(new Claim(ClaimTypes.NameIdentifier, model.Id.ToString()));                    
             claims.Add(new Claim(ClaimTypes.Name, model.UserName));
             foreach (var role in model.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }         
                                 
            var tokenDesriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                IssuedAt= DateTime.Now,
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
                var token = tokenHandler.CreateToken(tokenDesriptor);
                return tokenHandler.WriteToken(token);
        }
    }
}