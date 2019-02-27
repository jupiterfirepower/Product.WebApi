using Microsoft.EntityFrameworkCore;
using Product.WebApi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WebApiContrib.Core;

namespace Product.WebApi.DataAccess
{
    public class ProductsContext : DbContext
    {
        public ProductsContext()
        {
        }

        public ProductsContext(DbContextOptions<ProductsContext> options)
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
                .Property(p => p.RowVersion)
                .IsFixedLength()
                .HasMaxLength(8)
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();

            modelBuilder.Entity<Models.Product>()
                .Property(e => e.ProductName)
                .HasMaxLength(150);

            modelBuilder.Entity<Models.Product>()
                .Property(e => e.Description)
                .HasMaxLength(250);

            // configures one-to-many relationship
            modelBuilder.Entity<Models.Product>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Products);
                //.OnDelete(DeleteBehavior.Cascade);

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

            modelBuilder.Entity<Category>()
                .HasKey(p => p.CategoryId);

            modelBuilder.Entity<Category>()
                .Property(e => e.Name)
                .HasMaxLength(100);

            modelBuilder.Entity<Category>()
                .Property(e => e.Description)
                .HasMaxLength(150);

            modelBuilder.Entity<Category>(entity =>
            {
                entity
                    .HasMany(e => e.Children)
                    .WithOne(e => e.Parent)
                    .HasForeignKey(e => e.ParentId);
            });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ProductOwner> ProductOwners { get; set; }
        public DbSet<Manufacturer> Producers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Models.Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=products.db");
        }
    }
}
