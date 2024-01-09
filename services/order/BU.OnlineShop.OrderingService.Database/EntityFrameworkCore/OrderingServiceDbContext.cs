using BU.OnlineShop.OrderingService.Orders;
using Microsoft.EntityFrameworkCore;

namespace BU.OnlineShop.OrderingService.EntityFrameworkCore
{
    public class OrderingServiceDbContext : DbContext
    {
        public OrderingServiceDbContext(DbContextOptions<OrderingServiceDbContext> options) : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>(b =>
            {

            });

            modelBuilder.Entity<OrderItem>(b =>
            {
                b.Property(p => p.ProductName).HasMaxLength(OrderConsts.MaxProductNameLength).IsRequired();
                b.Property(p => p.ProductCode).HasMaxLength(OrderConsts.MaxProductCodeLength).IsRequired();
            });
        }
    }
}
