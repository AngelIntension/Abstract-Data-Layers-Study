using DataLayer;
using DataLayer.EFCore;
using DomainLayer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using Xunit;

namespace DomainLayer.Tests
{
    public class StockServiceTest
    {
        protected IProductRepository productRepository;

        public StockServiceTest()
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder<ProductContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(builder => builder.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            var context = new ProductContext(builder.Options);

            productRepository = new ProductRepository(context);
        }

        public class AddStock : StockServiceTest
        {
            [Fact]
            public void ShouldAddSpecifiedAmountToSpecifiedProduct()
            {
                // arrange
                productRepository.Insert(new() { Name = "Product 1", QuantityInStock = 10 });

                var sut = new StockService(productRepository);

                // act
                int result = sut.AddStock(productId: 1, amount: 10);

                // assert
                Assert.Equal(20, result);
                var product = productRepository.FindById(1);
                Assert.Equal(20, product.QuantityInStock);
            }
        }

        public class RemoveStock : StockServiceTest
        {
            [Fact]
            public void ShouldRemoveSpecifiedAmountFromSpecifiedProduct()
            {
                // arrange
                productRepository.Insert(new() { Name = "Product 1", QuantityInStock = 20 });

                var sut = new StockService(productRepository);

                // act
                int result = sut.RemoveStock(productId: 1, amount: 10);

                // assert
                Assert.Equal(10, result);
                var product = productRepository.FindById(1);
                Assert.Equal(10, product.QuantityInStock);
            }

            [Fact]
            public void ShouldThrowNotEnoughStockExceptionGivenAmountGreaterThanQuantityInStock()
            {
                // arrange
                productRepository.Insert(new() { Name = "Product 1", QuantityInStock = 20 });

                var sut = new StockService(productRepository);

                // act
                var exception = Assert.Throws<NotEnoughStockException>(() => sut.RemoveStock(1, 100));

                // assert
                Assert.Equal(20, exception.QuantityInStock);
                Assert.Equal(100, exception.AmountToRemove);
            }
        }
    }
}
