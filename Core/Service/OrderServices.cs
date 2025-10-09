using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.OrderModule;
using DomainLayer.Models.ProductModule;
using Shared.DataTransferObjects.IdentityDTos;
using Shared.DataTransferObjects.OrderDTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class OrderProfile(IMapper _mapper, IBasketRepository _basketRepository, IUnitOfWork _unitOfWork) : Profile
    {
        public async Task<OrderToReturnDTo> CreateOrder(OrderDTo orderDTo, string Email)
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
    }
}
