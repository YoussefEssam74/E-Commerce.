using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    internal class ProductWithBrandAndTypeSpecifications : BaseSpecifications<Product, int>
    {
        //Get All Products With Brands And Types
        public ProductWithBrandAndTypeSpecifications(int? BrandId, int? TypeId)
               : base(CriteriaExpression: P => (!BrandId.HasValue || P.BrandId == BrandId) &&
                                    (!TypeId.HasValue || P.TypeId == TypeId))
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);
        }
        // Get All Products By Id
        public ProductWithBrandAndTypeSpecifications(int id) : base(P => P.Id==id)
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);
        }
    }
}
