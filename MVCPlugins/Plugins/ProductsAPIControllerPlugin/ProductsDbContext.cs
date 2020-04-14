using Microsoft.EntityFrameworkCore;
using ProductsAPIControllerPlugin.Models;

namespace ProductsAPIControllerPlugin
{
    public class ProductsDbContext : DbContext
    {
        public virtual DbSet<Product> Products { get; set; }

        public ProductsDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
