using System.Collections.Generic;

namespace DataLayer
{
    public interface IProductRepository
    {
        IEnumerable<Product> All();
        void DeleteById(int productId);
        Product FindById(int productId);
        void Insert(Product product);
        void Update(Product product);
    }
}