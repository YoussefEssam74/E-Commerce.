using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.OrderModule;
using DomainLayer.Models.ProductModule;
using Service.Specifications;
using ServiceAbstraction;
using Shared.DataTransferObjects.IdentityDTos;
using Shared.DataTransferObjects.OrderDTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class OrderService(IMapper _mapper, IBasketRepository _basketRepository, IUnitOfWork _unitOfWork) : IOrderService
    {
        public async Task<OrderToReturnDTo> CreateOrderAsync(OrderDTo orderDTo, string Email)
        {
            // Map Address To Order Address
            var OrderAddress = _mapper.Map<AddressDTo, OrderAddress>(orderDTo.Address);
            // Get Basket
            var Basket = await _basketRepository.GetBasketAsync(orderDTo.BasketId)
                ?? throw new BasketNotFoundException(orderDTo.BasketId);

            // Create OrderItem List
            List<OrderItem> OrderItems = [];
            var ProductRepo = _unitOfWork.GetRepository<Product, int>();
            foreach (var item in Basket.Items)
            {
                var Product = await ProductRepo.GetByIdAsync(id: item.Id)
                ?? throw new ProductNotFoundException(item.Id);
                OrderItems.Add(CreateOrderItem(item, Product));
            }
                // Get Delivery Method
                var DeliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDTo.DeliveryMethodId)
                      ?? throw new DeliveryMethodNotFoundException(orderDTo.DeliveryMethodId);
                // Calculate Sub Total
                var SubTotal = OrderItems.Sum(i => i.Quantity * i.Price);

            var order = new Order(Email, OrderAddress, DeliveryMethod, OrderItems, SubTotal);
            await _unitOfWork.GetRepository<Order, Guid>().AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<Order, OrderToReturnDTo>(order);
        }

        private static OrderItem CreateOrderItem(DomainLayer.Models.BasketModule.BasketItem item, Product Product)
        {
            return new OrderItem()
            {
                Product = new ProductItemOrderd
                {
                    ProductId = Product.Id,
                    PictureUrl = Product.PictureUrl,
                    ProductName = Product.Name,

                },
                Price = Product.Price,
                Quantity = item.Quantity

            };
        }

        public async Task<IEnumerable<OrderToReturnDTo>> GetAllOrdersAsync(string Email)
        {
            var spec = new OrderSpecifications(Email);
            var orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(spec);
            var result = _mapper.Map<IEnumerable<Order>,IEnumerable<OrderToReturnDTo>>(orders);
            return result;
        }

        public async Task<OrderToReturnDTo> GetOrderByIdAsync(Guid id)
        {
            var spec = new OrderSpecifications(id);
            var order= await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(spec);
            if (order is null) throw new OrderNotFoundException(id);
            var result = _mapper.Map<Order,OrderToReturnDTo>(order);
            return result;
        }

        public async Task<IEnumerable<DeliveryMethodDTo>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<DeliveryMethodDTo>>(deliveryMethods);
            return result;
        }
    }
}
