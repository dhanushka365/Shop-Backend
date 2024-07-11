using Microsoft.EntityFrameworkCore;
using Shop_Backend.Models;

namespace Shop_Backend.Infrastructure
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring
        (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "ShopDb");       
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
