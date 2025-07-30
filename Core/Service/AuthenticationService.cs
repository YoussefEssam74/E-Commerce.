using DomainLayer.Exceptions;
using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using ServiceAbstraction;
using Shared.DataTransferObjects.IdentityDTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager) : IAuthenticationService
    {
        public async Task<UserDTo> LoginAsync(LoginDTo loginDTo)
        {
            // Check if email exists
            var user = await _userManager.FindByEmailAsync(loginDTo.Email)
                       ?? throw new UserNotFoundException(loginDTo.Email);

            // Check if password is valid
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDTo.Password);
            if (!isPasswordValid)
            return new UserDTo() { DisplayName = user.DisplayName, Email = user.Email,Token = CreateTokenAsync(user) };
            else
                throw new UnauthorizedException();
        }


        public async Task<UserDTo> RegisterAsync(RegisterDTo registerDTo)
        {
            // Mapping Register Dto => Application User
            var user = new ApplicationUser()
            {
                DisplayName = registerDTo.DisplayName,
                Email =registerDTo.Email,
                PhoneNumber = registerDTo.PhoneNumber,
                UserName = registerDTo.UserName
            };
            // Create User [Application User]
            var Result = await _userManager.CreateAsync( user,  registerDTo.Password);
            if (Result.Succeeded)
                return new UserDTo() { DisplayName = user.DisplayName, Email= user.Email, Token = CreateTokenAsync(user) };
            else
            {
                // Throw BadRequest Exceptions
                var Errors = Result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(Errors);
            }
          
        }
        private static string CreateTokenAsync(ApplicationUser user)
        {
            return "TOKEN - TODO";
        }
        }
}
