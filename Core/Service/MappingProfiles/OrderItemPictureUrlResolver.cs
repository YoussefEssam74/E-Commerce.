using AutoMapper;
using DomainLayer.Models.OrderModule;
using Microsoft.Extensions.Configuration;
using Shared.DataTransferObjects.OrderDTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfiles
{
    class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDTo, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDTo destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.Product.PictureUrl))
                return string.Empty;
            else
            {
                //var Url = $"https://localhost:7148/{source.PictureUrl}";
                var Url = $"{_configuration.GetSection("Urls")["BaseUrl"]}{source.Product.PictureUrl}";
                return Url;
            }
        }
}
