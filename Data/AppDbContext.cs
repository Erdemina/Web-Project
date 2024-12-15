using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using Web_Project.Models;

namespace Web_Project.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // User tablosu için DbSet
        public DbSet<User> Users { get; set; } // "Users" tablosu
        public DbSet<Property> Properties { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User tablosu yapılandırması
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId); // Primary Key
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Role).HasMaxLength(20).HasDefaultValue("user");
                entity.Property(u => u.CreatedAt).HasDefaultValueSql("GETDATE()");
            });

            // Properties tablosu yapılandırması
            modelBuilder.Entity<Property>(entity =>
            {
                entity.HasKey(p => p.propid); // Primary Key tanımlaması
                entity.Property(p => p.title).IsRequired().HasMaxLength(100);
                entity.Property(p => p.description).HasMaxLength(500);
                entity.Property(p => p.location).HasMaxLength(255);
                entity.Property(p => p.pricenight).IsRequired();
                entity.Property(p => p.rating).HasDefaultValue(0);
                entity.Property(p => p.CreatedAt).HasDefaultValueSql("GETDATE()");
            });


        }
    }
}
