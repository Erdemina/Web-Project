using Microsoft.EntityFrameworkCore;
using Web_Project.Models;

namespace Web_Project.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Tablolar
        public DbSet<User> Users { get; set; } // "Users" tablosu
        public DbSet<Property> Properties { get; set; } // "Properties" tablosu
        public DbSet<PropertyView> PropertyViews { get; set; } // "PropertyViews" tablosu
        public DbSet<Reviews> Reviews { get; set; } // "Reviews" tablosu
        public DbSet<Booking> Bookings { get; set; }
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

                // Role alanını enum (string) olarak saklama
                entity.Property(u => u.Role)
                      .HasConversion<string>() // Enum'ı string olarak saklar
                      .HasDefaultValue(UserRole.User); // Varsayılan değer: User

                entity.Property(u => u.CreatedAt).HasDefaultValueSql("GETDATE()");
            });

            // Properties tablosu yapılandırması
            modelBuilder.Entity<Property>(entity =>
            {
                entity.HasKey(p => p.propid); // Primary Key
                entity.Property(p => p.title).IsRequired().HasMaxLength(100);
                entity.Property(p => p.description).HasMaxLength(500);
                entity.Property(p => p.location).HasMaxLength(255);
                entity.Property(p => p.pricenight).IsRequired();
                entity.Property(p => p.rating).HasDefaultValue(0);
                entity.Property(p => p.guests).HasDefaultValue(1);
                entity.Property(p => p.CreatedAt).HasDefaultValueSql("GETDATE()");

                // Property - PropertyView ilişkisi
                entity.HasMany(p => p.PropertyViews)
                      .WithOne(v => v.Property)
                      .HasForeignKey(v => v.PropertyId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // PropertyViews tablosu yapılandırması
            modelBuilder.Entity<PropertyView>(entity =>
            {
                entity.HasKey(v => v.ViewId); // Primary Key
                entity.Property(v => v.ViewedAt).IsRequired();
            });

            // Reviews tablosu yapılandırması
            modelBuilder.Entity<Reviews>(entity =>
            {
                entity.HasKey(r => r.ReviewId); // Primary Key
                entity.Property(r => r.PropertyId).HasColumnName("property_id"); // Doğru sütun adı
                entity.Property(r => r.UserId).HasColumnName("user_id");
                entity.Property(r => r.rating).IsRequired();
                entity.Property(r => r.comment).HasMaxLength(500);
                entity.Property(r => r.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}
