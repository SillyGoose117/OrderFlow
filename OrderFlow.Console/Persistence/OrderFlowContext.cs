using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderFlow.Console.Models;

namespace OrderFlow.Console.Persistence;

public class OrderFlowContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Customer> Customers => Set<Customer>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options
            .UseSqlite("Data Source=orderflow.db")
            .LogTo(System.Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging(); //Do wglądu na potrzeby Labu!
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(c => c.FullName);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasMany(o => o.Items)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(o => o.CurrentStatus);

            entity.Ignore(o => o.TotalAmount);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasOne(oi => oi.Product)
                .WithMany(p => p.OrderedItems)
                .HasForeignKey(oi => oi.ProductId);
            
            entity.Ignore(oi => oi.TotalPrice);
            entity.Property(oi => oi.UnitPrice)
                .HasPrecision(18, 2);
        });
        
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(p => p.Price).HasPrecision(18, 2);
        });
    }
}