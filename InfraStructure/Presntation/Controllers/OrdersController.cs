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
    [Authorize]

    public class OrdersController(IServiceManager _serviceManager) : ApiBaseController
    {
        // Create Order
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDTo>> CreateOrder(OrderDTo orderDTo)
        {
            // Get Email From Token
            var Order = await _serviceManager.orderService.CreateOrderAsync(orderDTo, GetEmailFromToken());
            return Ok(Order);
        }

        // Get Delivery Methods
        [AllowAnonymous]
        [HttpGet("DeliveryMethods")] // GET BaseUrl/api/Orders/DeliveryMethods
        public async Task<ActionResult<IEnumerable<DeliveryMethodDTo>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _serviceManager.orderService.GetDeliveryMethodsAsync();
            return Ok(deliveryMethods);
        }

        // Get All Orders By Email
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDTo>>> GetAllOrders()
        {
            var orders = await _serviceManager.orderService.GetAllOrdersAsync(GetEmailFromToken());
            return Ok(orders);
        }

        // Get Order By Id
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDTo>> GetOrderById(Guid id)
        {
            var order = await _serviceManager.orderService.GetOrderByIdAsync(id);
            return Ok(order);
        }
    }
}
