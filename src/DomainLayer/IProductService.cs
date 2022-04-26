using System.Collections.Generic;

namespace DomainLayer
{
    public interface IProductService
    {
        IEnumerable<Product> All();
    }
}