using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using Web_Project.models;

namespace Web_Project.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // User tablosu için DbSet
        public DbSet<User> Users { get; set; } // "Users" tablosu

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
        }
    }
}
