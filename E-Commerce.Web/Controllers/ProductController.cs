using E_Commerce.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.Controllers
{
    [Route("api/[controller]")] // BaseUrl/api/Product
    [ApiController]
    public class ProductController : ControllerBase
    {
        // Get : BaseUrl/Api/Product/10
        [HttpGet("{id}")]
        public ActionResult<Product> Get(int id)
        {
            return  new Product () { Id = id };
        }
        // Get : BaseUrl/api/Product
        [HttpGet]
        public ActionResult<Product> GetAll()
        {
            return new Product() { Id = 100 };
        }

        [HttpPost]
        public ActionResult<Product> AddProduct(Product product)
        {
            return new Product();
        }

        [HttpPut]
        public ActionResult<Product> UpdateProduct(Product product)
        {
            return new Product();
        }
        [HttpDelete]
        public ActionResult<Product> DeleteProduct(Product product)
        {
            return new Product();
        }
    }
}
