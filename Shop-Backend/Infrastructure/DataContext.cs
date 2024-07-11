using Microsoft.EntityFrameworkCore;

namespace Shop_Backend.Infrastructure
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring
        (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "ShopDb");
        }
    }
}
