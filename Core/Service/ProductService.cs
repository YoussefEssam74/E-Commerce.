﻿using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Models;
using Service.Specifications;
using ServiceAbstraction;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ProductService(IUnitOfWork _unitOfWork,IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<BrandDTo>> GetAllBrandsAsync()
        {
            var Repo = _unitOfWork.GetRepository<ProductBrand, int>();
            var Brands = await Repo.GetAllAsync();
            var BrandsDto = _mapper.Map<IEnumerable<ProductBrand>,IEnumerable<BrandDTo>>(Brands);
            return BrandsDto;
        }

        public async Task<IEnumerable<ProductDTo>> GetAllProductsAsync(int? BrandId, int? TypeId)
        {
            var Specifications = new ProductWithBrandAndTypeSpecifications( BrandId, TypeId);
            var Products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(Specifications);

            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTo>>(Products);
        }
               
        public async Task<IEnumerable<TypeDTo>> GetAllTypesAsync()
        {
            var Types =await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var TypesDto = _mapper.Map < IEnumerable < ProductType >, IEnumerable < TypeDTo >> ( Types);
            return TypesDto;
        }

        public async  Task<ProductDTo> GetProductByIdAsync(int id)
        {
            var Specifications = new ProductWithBrandAndTypeSpecifications(id);

            var Product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(Specifications);
            return _mapper.Map<Product, ProductDTo>( Product);
        }
    }
}
