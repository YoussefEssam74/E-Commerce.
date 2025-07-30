using DomainLayer.Exceptions;
using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using Shared.DataTransferObjects.IdentityDTos;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager,IConfiguration _configuration) : IAuthenticationService
    {
        public async Task<UserDTo> LoginAsync(LoginDTo loginDTo)
        {
            // Check if email exists
            var user = await _userManager.FindByEmailAsync(loginDTo.Email)
                       ?? throw new UserNotFoundException(loginDTo.Email);

            // Check if password is valid
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDTo.Password);
            if (!isPasswordValid)
                return new UserDTo() { DisplayName = user.DisplayName, Email = user.Email, Token = await CreateTokenAsync(user) };
            else
                throw new UnauthorizedException();
        }


        public async Task<UserDTo> RegisterAsync(RegisterDTo registerDTo)
        {
            // Mapping Register Dto => Application User
            var user = new ApplicationUser()
            {
                DisplayName = registerDTo.DisplayName,
                Email = registerDTo.Email,
                PhoneNumber = registerDTo.PhoneNumber,
                UserName = registerDTo.UserName
            };
            // Create User [Application User]
            var Result = await _userManager.CreateAsync(user, registerDTo.Password);
            if (Result.Succeeded)
                return new UserDTo() { DisplayName = user.DisplayName, Email = user.Email, Token = await CreateTokenAsync(user) };
            else
            {
                // Throw BadRequest Exceptions
                var Errors = Result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(Errors);
            }

        }


        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
        new Claim(ClaimTypes.Email, user.Email!),
        new Claim(ClaimTypes.Name, user.UserName!),
        new Claim(ClaimTypes.NameIdentifier, user.Id!)
        };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }            
            var secretKey = _configuration.GetSection("JWTOptions")["SecretKey"]; 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWTOptions:Issuer"],
                audience: _configuration["JWTOptions:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
    
