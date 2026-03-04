using Microsoft.EntityFrameworkCore;
using MINT.EShop.Core.Entities;
using MINT.EShop.Core.Entities.Order;

namespace MINT.EShop.Infrastracture
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}