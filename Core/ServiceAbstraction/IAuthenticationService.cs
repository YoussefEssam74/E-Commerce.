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
    }
}
