using AutoMapper;
using DomainLayer.Models;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTo>()
                       .ForMember(dist => dist.BrandName, options=> options.MapFrom(src => src.ProductBrand.Name))
                       .ForMember(dist => dist.TypeName, options => options.MapFrom(src => src.ProductType.Name));
            CreateMap<ProductType, TypeDTo>();
            CreateMap<ProductBrand, BrandDTo>();
        }
    }
}
