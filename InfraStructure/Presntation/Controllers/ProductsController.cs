using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presntation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]   //BaseUrl/api/Products
    public class ProductsController(IServiceManager _serviceManager) : ControllerBase
    {
        // Get All Products
        //Get BaseUrl/api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTo>>> GetAllProducts([FromQuery]ProductQueryParams queryParams)
        {
           var Products = await _serviceManager.ProductService.GetAllProductsAsync(queryParams);
            return Ok(Products);
        }

        //Get Product By Id
        //Get BaseUrl/api/Products/10
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTo>> GetProduct(int id)
        {
            var Products = await _serviceManager.ProductService.GetProductByIdAsync(id);
            return Ok(Products);
        }

        //Get all types
        //Get BaseUrl/api/Products/types
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<TypeDTo>>> GetTypes()
        {
            var Types = await _serviceManager.ProductService.GetAllTypesAsync();
            return Ok(Types);
        }
        //Get all brands
        //Get BaseUrl/api/Products/brands
        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<BrandDTo>>> GetBrands()
        {
            var Brands = await _serviceManager.ProductService.GetAllBrandsAsync();
            return Ok(Brands);
        }


    }

}
