using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DataLayer.EFCore
{
    public class ProductRepository
    {
        private ProductContext context;

        public ProductRepository([NotNull] ProductContext context)
        {
            this.context = context;
        }

        public IEnumerable<Product> All()
        {
            return context.Products;
        }

        public void DeleteById(int productId)
        {
            var product = context.Products.Find(productId);
            context.Products.Remove(product);
            context.SaveChanges();
        }

        public Product FindById(int productId)
        {
            return context.Products.Find(productId);
        }

        public void Insert(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
        }

        public void Update(Product product)
        {
            context.Products.Update(product);
            context.SaveChanges();
        }
    }
}
