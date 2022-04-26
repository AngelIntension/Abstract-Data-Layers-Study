using System.Collections.Generic;

namespace DomainLayer
{
    public class Product
    {
        public int Id { get; set; }
        public IEnumerable<char> Name { get; set; }
    }
}
