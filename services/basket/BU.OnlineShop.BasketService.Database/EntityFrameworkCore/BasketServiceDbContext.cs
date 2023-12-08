using BU.OnlineShop.BasketService.Domain.Baskets;
using Microsoft.EntityFrameworkCore;

namespace BU.OnlineShop.BasketService.Database.EntityFrameworkCore
{
    public class BasketServiceDbContext : DbContext
    {
        public BasketServiceDbContext(DbContextOptions<BasketServiceDbContext> options) : base(options)
        {

        }

        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketLine> BasketLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Basket>(b =>
            {
                b.HasMany(p => p.BasketLines).WithOne().HasForeignKey(p=>p.BasketId).IsRequired();
            });

            modelBuilder.Entity<BasketLine>(b =>
            {
                b.Property(p => p.Count).HasMaxLength(int.MaxValue).IsRequired();
            });
        }
    }
}
