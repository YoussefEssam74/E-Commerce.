using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTransferObjects.OrderDTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presntation.Controllers
{
    public class OrderController(IServiceManager _serviceManager) : ApiBaseController
    {
        // Create Order
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDTo>> CreateOrder(OrderDTo orderDTo)
        {
            // Get Email From Token
            var Order = await _serviceManager.orderService.CreateOrder(orderDTo, GetEmailFromToken());
            return Ok(Order);
        }
    }
}
