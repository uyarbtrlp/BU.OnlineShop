using BU.OnlineShop.FileService.Domain.FileInformations;
using BU.OnlineShop.FileService.Domain.Shared.FileInformations;
using Microsoft.EntityFrameworkCore;

namespace BU.OnlineShop.FileService.Database.EntityFrameworkCore
{
    public class FileServiceDbContext : DbContext
    {
        public FileServiceDbContext(DbContextOptions<FileServiceDbContext> options) : base(options)
        {

        }

        public DbSet<FileInformation> FileInformations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FileInformation>(b =>
            {
                b.Property(p => p.Name).HasMaxLength(FileInformationConsts.MaxNameLength).IsRequired();
                b.Property(p => p.MimeType).HasMaxLength(FileInformationConsts.MaxMimeTypeLength).IsRequired();
                b.Property(p => p.Size).HasMaxLength(FileInformationConsts.MaxSizeLength).IsRequired();
                b.Property(p => p.Content).HasMaxLength(FileInformationConsts.MaxContentLength).IsRequired();
            });
        }
    }
}
