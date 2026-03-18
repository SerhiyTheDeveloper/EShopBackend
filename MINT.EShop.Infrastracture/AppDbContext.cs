using Microsoft.EntityFrameworkCore;
using MINT.EShop.Core.Entities;
using MINT.EShop.Core.Entities.Order;
using MINT.EShop.Core.Entities.UserData;
using MINT.EShop.Infrastracture.Configurations;

namespace MINT.EShop.Infrastracture
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserCredential> UserCredentials { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserCredentialConfiguration());
            modelBuilder.ApplyConfiguration(new UserSessionConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}