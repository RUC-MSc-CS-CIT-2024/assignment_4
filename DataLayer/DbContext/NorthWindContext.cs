using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class NorthWindContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetails> OrderDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        optionsBuilder.UseNpgsql("host=localhost;db=northwind;User Id=idahayjorgensen;password=Jørgensen");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        MapCategories(modelBuilder);
        MapProducts(modelBuilder);
        MapOrders(modelBuilder);
        MapOrderDetails(modelBuilder);
    }

    private static void MapProducts(ModelBuilder modelBuilder)
    {
        // Products
        modelBuilder.Entity<Product>().ToTable("products");
        modelBuilder.Entity<Product>().Property(x => x.Id).HasColumnName("productid");
        modelBuilder.Entity<Product>().Property(x => x.Name).HasColumnName("productname");
        modelBuilder.Entity<Product>().Property(x => x.QuantityPerUnit).HasColumnName("quantityperunit");
        modelBuilder.Entity<Product>().Property(x => x.UnitPrice).HasColumnName("unitprice");
        modelBuilder.Entity<Product>().Property(x => x.UnitsInStock).HasColumnName("unitsinstock");
        modelBuilder.Entity<Product>().Property(x => x.CategoryId).HasColumnName("categoryid");
    }

    private static void MapCategories(ModelBuilder modelBuilder)
    {
        // Categories
        modelBuilder.Entity<Category>().ToTable("categories");
        modelBuilder.Entity<Category>().Property(x => x.Id).HasColumnName("categoryid");
        modelBuilder.Entity<Category>().Property(x => x.Name).HasColumnName("categoryname");
        modelBuilder.Entity<Category>().Property(x => x.Description).HasColumnName("description");
    }

    private static void MapOrders(ModelBuilder modelBuilder)
    {
        // Orders
        modelBuilder.Entity<Order>().ToTable("orders");
        modelBuilder.Entity<Order>().Property(x => x.Id).HasColumnName("orderid");
        modelBuilder.Entity<Order>().Property(x => x.Date).HasColumnName("orderdate");
        modelBuilder.Entity<Order>().Property(x => x.Required).HasColumnName("requireddate");
        modelBuilder.Entity<Order>().Property(x => x.ShipName).HasColumnName("shipname");
        modelBuilder.Entity<Order>().Property(x => x.ShipCity).HasColumnName("shipcity");
        modelBuilder.Entity<Order>().HasMany(o => o.OrderDetails).WithOne(od => od.Order).HasForeignKey(od => od.OrderId);
    }

    private static void MapOrderDetails(ModelBuilder modelBuilder)
    {
        // OrderDetails
        modelBuilder.Entity<OrderDetails>().ToTable("orderdetails").HasKey(od => new { od.OrderId, od.ProductId });
        modelBuilder.Entity<OrderDetails>().Property(od => od.UnitPrice).HasColumnName("unitprice");
        modelBuilder.Entity<OrderDetails>().Property(od => od.Quantity).HasColumnName("quantity");
        modelBuilder.Entity<OrderDetails>().Property(od => od.Discount).HasColumnName("discount");
        modelBuilder.Entity<OrderDetails>().Property(od => od.OrderId).HasColumnName("orderid");
        modelBuilder.Entity<OrderDetails>().Property(od => od.ProductId).HasColumnName("productid");
    }
}