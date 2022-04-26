using DomainLayer;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace PresentationLayer.Controllers
{
    [ApiController]
    [Route("/products/{productId}/")]
    public class StockController : ControllerBase
    {
        private readonly IStockService stockService;

        public StockController([NotNull] IStockService stockService)
        {
            this.stockService = stockService;
        }

        [HttpPost("add-stocks")]
        public ActionResult<StockLevel> AddStock(int productId, [FromBody] AddStockCommand command)
        {
            var quantityInStock = stockService.AddStock(productId, command.Amount);
            return Ok(new StockLevel(quantityInStock));
        }

        [HttpPost("remove-stocks")]
        public ActionResult<StockLevel> RemoveStock(int productId, [FromBody] RemoveStockCommand command)
        {
            try
            {
                var quantityInStock = stockService.RemoveStock(productId, command.Amount);
                return Ok(new StockLevel(quantityInStock));
            }
            catch(NotEnoughStockException ex)
            {
                return Conflict(new
                {
                    ex.Message,
                    ex.QuantityInStock,
                    ex.AmountToRemove
                });
            }
        }

        public class StockLevel
        {
            public StockLevel(int quantityInStock)
            {
                QuantityInStock = quantityInStock;
            }

            public int QuantityInStock { get; set; }
        }

        public class AddStockCommand
        {
            public int Amount { get; set; }
        }

        public class RemoveStockCommand
        {
            public int Amount { get; set; }
        }
    }
}
