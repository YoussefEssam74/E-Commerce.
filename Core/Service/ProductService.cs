using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Models;
using Service.Specifications;
using ServiceAbstraction;
using Shared;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<PaginationResult<ProductDTo>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var Specifications = new ProductWithBrandAndTypeSpecifications(queryParams);
            var AllProducts = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(specifications: Specifications);
            var Data=_mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTo>>( AllProducts);
            var ProductCount = AllProducts.Count();
            return new PaginationResult<ProductDTo>(queryParams.PageIndex, ProductCount, 0,Data);

            //var Specifications = new ProductWithBrandAndTypeSpecifications(queryParams);
            //var Products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(Specifications);

            //return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTo>>(Products);
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
