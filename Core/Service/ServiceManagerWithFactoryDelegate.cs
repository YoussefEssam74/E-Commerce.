using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceManagerWithFactoryDelegate(
            Func<IProductService> productFactory,
            Func<IBasketService> basketFactory,
            Func<IAuthenticationService> authenticationFactory,
            Func<IOrderService> orderFactory) : IServiceManager
    {
        public IProductService ProductService => productFactory.Invoke();

        public IBasketService BasketService => basketFactory.Invoke();

        public IAuthenticationService AuthenticationService => authenticationFactory.Invoke();
        public IOrderService orderService => orderFactory.Invoke();
    }
}
