using Microsoft.EntityFrameworkCore;
using YamSoft.API.Entities;

namespace YamSoft.API.Context;

public class YamSoftDbContext(DbContextOptions<YamSoftDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Username).IsUnique();
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.Price).HasPrecision(10, 2);
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasOne(c => c.User)
                  .WithOne(u => u.Cart)
                  .HasForeignKey<Cart>(c => c.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasOne(ci => ci.Cart)
                  .WithMany(c => c.CartItems)
                  .HasForeignKey(ci => ci.CartId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ci => ci.Product)
                  .WithMany()
                  .HasForeignKey(ci => ci.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(ci => new { ci.CartId, ci.ProductId }).IsUnique();
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasOne(n => n.User)
                  .WithMany(u => u.Notifications)
                  .HasForeignKey(n => n.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.Type)
                  .HasConversion<string>()
                  .HasMaxLength(50);
                  
            entity.Property(e => e.Status)
                  .HasConversion<string>()
                  .HasMaxLength(20);
                  
            entity.Property(e => e.Message).HasMaxLength(1000);
        });
    }
}