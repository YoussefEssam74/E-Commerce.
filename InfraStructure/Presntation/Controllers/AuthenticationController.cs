using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTransferObjects.IdentityDTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presntation.Controllers
{
    public class AuthenticationController(IServiceManager _serviceManager): ApiBaseController
    {
        [HttpPost("مogin")]
        public async Task<ActionResult<UserDTo>> Login( LoginDTo loginDTo)
        {
            var User = await _serviceManager.AuthenticationService.LoginAsync(loginDTo);


            return Ok(User);
        }

        [HttpPost("قegister")]
        public async Task<ActionResult<UserDTo>> Register(RegisterDTo registerDTo)
        {
            var User = await _serviceManager.AuthenticationService.RegisterAsync(registerDTo);
            return Ok(User);
        }
    }
}
