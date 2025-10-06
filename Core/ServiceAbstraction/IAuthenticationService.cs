using Shared.DataTransferObjects.IdentityDTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IAuthenticationService
    {
        // Login
        // Take Email and Password Then Return Token Email and DisplayName
        Task<UserDTo> LoginAsync(LoginDTo loginDTo);
        // Register
        // Take Email, Password UserName Display Name And Phone Number
        // Then Return Token Email and Display Name 1
        Task<UserDTo> RegisterAsync(RegisterDTo registerDTo);


        // Check Email
        // Take string Email Then Return bool boolean
        
        Task<bool> CheckEmailAsync(string Email);
        // Get Current User Address
        // Take string Email Then Return AddressDTO
        
        Task<AddressDto> GetCurrentUserAddressAsync(string Email);
        // Update Current User Address
        // Take AdressDTO Updated Address and string Email Then Return AdressDTO Address after Update
       
        Task<AddressDto> UpdateCurrentUserAddressAsync(string email, AddressDto addressDto);
        // Get Current User
        // Take string Email Then Return UserDTo Token Email and Display Name
        Task<UserDTo> GetCurrentUserAsync(string Email);
    }
}
