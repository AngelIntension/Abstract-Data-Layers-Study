using DomainLayer;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace PresentationLayer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController([NotNull] IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public ActionResult<ProductDetails> Get()
        {
            var products = productService.All()
                .Select(p => new ProductDetails(p.Id, p.Name, p.QuantityInStock));
            return Ok(products);
        }

        public class ProductDetails
        {
            public ProductDetails(int id, string name, int quantityInStock)
            {
                Id = id;
                Name = name;
                QuantityInStock = quantityInStock;
            }

            public int Id { get; }
            public string Name { get; }
            public int QuantityInStock { get; }
        }
    }
}
