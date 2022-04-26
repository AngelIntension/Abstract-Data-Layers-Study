using DataLayer;
using System.Diagnostics.CodeAnalysis;

namespace DomainLayer.Services
{
    public class StockService
    {
        private readonly IProductRepository repository;

        public StockService([NotNull] IProductRepository repository)
        {
            this.repository = repository;
        }

        public int AddStock(int productId, int amount)
        {
            var product = repository.FindById(productId);
            product.QuantityInStock += amount;
            repository.Update(product);
            return product.QuantityInStock;
        }

        public int RemoveStock(int productId, int amount)
        {
            var product = repository.FindById(productId);
            if(amount > product.QuantityInStock)
            {
                throw new NotEnoughStockException(quantityInStock: product.QuantityInStock, amountToRemove: amount);
            }
            product.QuantityInStock -= amount;
            repository.Update(product);
            return product.QuantityInStock;
        }
    }
}
