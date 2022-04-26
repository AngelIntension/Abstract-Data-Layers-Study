using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using Xunit;

namespace DataLayer.EFCore.Tests
{
    public class ProductRepositoryTest
    {
        private readonly DbContextOptionsBuilder builder = new DbContextOptionsBuilder<ProductContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(builder => builder.Ignore(InMemoryEventId.TransactionIgnoredWarning));

        public class All : ProductRepositoryTest
        {
            [Fact]
            public void ShouldReturnAllProducts()
            {
                // arrange
                using var arrangeContext = new ProductContext(builder.Options);
                using var actContext = new ProductContext(builder.Options);

                arrangeContext.Products.Add(new() { Name = "Product 1" });
                arrangeContext.Products.Add(new() { Name = "Product 2" });
                arrangeContext.SaveChanges();

                var sut = new ProductRepository(actContext);

                // act
                IEnumerable<Product> result = sut.All();

                // assert
                Assert.Collection(result,
                    product => Assert.Equal("Product 1", product.Name),
                    product => Assert.Equal("Product 2", product.Name)
                );
            }
        }

        public class DeleteById : ProductRepositoryTest
        {
            [Fact]
            public void ShouldDeleteTheSpecifiedProduct()
            {
                // arrange
                using var arrangeContext = new ProductContext(builder.Options);
                using var actContext = new ProductContext(builder.Options);
                using var assertContext = new ProductContext(builder.Options);

                arrangeContext.Products.Add(new() { Name = "Product 1" });
                arrangeContext.Products.Add(new() { Name = "Product 2" });
                arrangeContext.SaveChanges();

                var sut = new ProductRepository(actContext);

                // act
                sut.DeleteById(productId: 1);

                // assert
                var products = assertContext.Products;
                Assert.Collection(products, product => Assert.Equal("Product 2", product.Name));
            }
        }

        public class FindById : ProductRepositoryTest
        {
            [Fact]
            public void ShouldReturnTheSpecifiedProduct()
            {
                // arrange
                using var arrangeContext = new ProductContext(builder.Options);
                using var actContext = new ProductContext(builder.Options);

                arrangeContext.Products.Add(new() { Name = "Product 1" });
                arrangeContext.Products.Add(new() { Name = "Product 2" });
                arrangeContext.SaveChanges();

                var sut = new ProductRepository(actContext);

                // act
                Product result = sut.FindById(productId: 2);

                // assert
                Assert.Equal("Product 2", result.Name);
            }
        }

        public class Insert : ProductRepositoryTest
        {
            [Fact]
            public void ShouldInsertGivenProduct()
            {
                // arrange
                using var actContext = new ProductContext(builder.Options);
                using var assertContext = new ProductContext(builder.Options);

                var product = new Product { Name = "Product 1" };

                var sut = new ProductRepository(actContext);

                // act
                sut.Insert(product);

                // assert
                var result = assertContext.Products.Find(1);
                Assert.Equal("Product 1", result.Name);
            }
        }

        public class Update : ProductRepositoryTest
        {
            [Fact]
            public void ShouldUpdateSpecifiedProduct()
            {
                // arrange
                using var arrangeContext = new ProductContext(builder.Options);
                using var actContext = new ProductContext(builder.Options);
                using var assertContext = new ProductContext(builder.Options);

                arrangeContext.Products.Add(new() { Name = "Product 1" });
                arrangeContext.SaveChanges();

                var sut = new ProductRepository(actContext);

                // act
                var product = actContext.Products.Find(1);
                product.Name = "New Name";
                sut.Update(product);

                // assert
                var result = assertContext.Products.Find(1);
                Assert.Equal("New Name", result.Name);
            }
        }
    }
}
