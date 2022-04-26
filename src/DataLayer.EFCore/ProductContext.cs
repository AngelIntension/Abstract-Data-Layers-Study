using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DataLayer.EFCore
{
    public class ProductContext : DbContext
    {
        public ProductContext([NotNull] DbContextOptions options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}
