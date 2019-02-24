using Microsoft.EntityFrameworkCore;
using Product.WebApi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product.WebApi.Tests
{
    public class ProductsTestContext : DbContext
    {
        public ProductsTestContext()
        {
        }

        public ProductsTestContext(DbContextOptions<ProductsTestContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Product>()
                .HasKey(p => p.ProductId);

            modelBuilder.Entity<Models.Product>()
                .Property(p => p.ProductId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Models.Product>()
                .Property(e => e.ProductName)
                .HasMaxLength(150);

            modelBuilder.Entity<Models.Product>()
                .Property(e => e.Description)
                .HasMaxLength(250);

            modelBuilder.Entity<ProductOwner>()
                .HasKey(p => p.OwnerId);

            modelBuilder.Entity<ProductOwner>()
                .Property(e => e.OwnerName)
                .HasMaxLength(150);

            modelBuilder.Entity<ProductOwner>()
                .Property(e => e.Address)
                .HasMaxLength(100);

            modelBuilder.Entity<ProductOwner>()
                .Property(e => e.Email)
                .HasMaxLength(50);

            modelBuilder.Entity<ProductOwner>()
                .Property(e => e.Phone)
                .HasMaxLength(50);

            modelBuilder.Entity<Manufacturer>()
                .HasKey(p => p.ManufacturerId);

            modelBuilder.Entity<Manufacturer>()
                .Property(e => e.ManufactureName)
                .HasMaxLength(150);

            modelBuilder.Entity<Manufacturer>()
                .Property(e => e.Address)
                .HasMaxLength(100);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Models.Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=products.db");
        }
    }
}
