using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IProductService
    {
        // GetAll Products 

        Task<IEnumerable<ProductDTo>> GetAllProductsAsync();
        // get Product By Id 

        Task<ProductDTo> GetProductByIdAsync(int Id);
        // get All Types 

        Task<IEnumerable<TypeDTo>> GetAllTypesAsync();
        // get All Brands 

        Task<IEnumerable<BrandDTo>> GetAllBrandsAsync();
    }
}
