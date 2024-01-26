﻿using BU.OnlineShop.CatalogService.Categories;
using BU.OnlineShop.CatalogService.Products;
using Microsoft.EntityFrameworkCore;

namespace BU.OnlineShop.CatalogService.EntityFrameworkCore
{
    public class CatalogServiceDbContext : DbContext
    {
        public CatalogServiceDbContext(DbContextOptions<CatalogServiceDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(b =>
            {
                b.Property(p => p.Code).HasMaxLength(ProductConsts.MaxCodeLength).IsRequired();
                b.Property(p => p.Name).HasMaxLength(ProductConsts.MaxNameLength).IsRequired();
            });

            modelBuilder.Entity<Category>(b =>
            {
                b.Property(p => p.Name).HasMaxLength(CategoryConsts.MaxNameLength).IsRequired();
                b.Property(p => p.Description).HasMaxLength(CategoryConsts.MaxDescriptionLength);
            });
        }
    }
}