using DataLayer;
using DataLayer.EFCore;
using DomainLayer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using Xunit;

namespace DomainLayer.Tests
{
    public class ProductServiceTest
    {
        protected readonly IProductRepository productRepository;

        public ProductServiceTest()
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder<ProductContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(builder => builder.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            var context = new ProductContext(builder.Options);

            productRepository = new ProductRepository(context);
        }

        public class All : ProductServiceTest
        {
            [Fact]
            public void ShouldReturnAllProducts()
            {
                // arrange
                productRepository.Insert(new() { Name = "Product 1" });
                productRepository.Insert(new() { Name = "Product 2" });

                var sut = new ProductService(productRepository);

                // act
                IEnumerable<Product> result = sut.All();

                // assert
                Assert.Collection(result,
                    product => Assert.Equal("Product 1", product.Name),
                    product => Assert.Equal("Product 2", product.Name)
                );
            }
        }
    }
}
