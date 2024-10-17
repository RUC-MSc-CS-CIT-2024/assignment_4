using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class NorthwindContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    
    public DbSet<Product> Products { get; set; }
    
    public DbSet<Order> Orders { get; set; }
    
    public DbSet<OrderDetails> OrderDetails { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        optionsBuilder.UseNpgsql("host=localhost;uid=postgres;pwd=postgres;port=5433;db=northwind");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Categories
        modelBuilder.Entity<Category>().ToTable("categories");
        modelBuilder.Entity<Category>().HasKey(x => x.Id);
        modelBuilder.Entity<Category>().Property(x => x.Id).HasColumnName("categoryid");
        modelBuilder.Entity<Category>().Property(x => x.Name).HasColumnName("categoryname");
        modelBuilder.Entity<Category>().Property(x => x.Description).HasColumnName("description");

        //Products
        modelBuilder.Entity<Product>().ToTable("products");
        modelBuilder.Entity<Product>().HasKey(x => x.Id);
        modelBuilder.Entity<Product>().HasOne(p => p.Category).WithMany().HasForeignKey(p => p.CategoryId);
        modelBuilder.Entity<Product>().Property(x => x.Id).HasColumnName("productid");
        modelBuilder.Entity<Product>().Property(x => x.Name).HasColumnName("productname");
        modelBuilder.Entity<Product>().Property(x => x.CategoryId).HasColumnName("categoryid");
        modelBuilder.Entity<Product>().Property(x => x.QuantityPerUnit).HasColumnName("quantityperunit");
        modelBuilder.Entity<Product>().Property(x => x.UnitPrice).HasColumnName("unitprice");
        modelBuilder.Entity<Product>().Property(x => x.UnitsInStock).HasColumnName("unitsinstock");
        
        //Orders
        modelBuilder.Entity<Order>().ToTable("orders");
        modelBuilder.Entity<Order>().HasKey(x => x.Id);
        modelBuilder.Entity<Order>().Property(x => x.Id).HasColumnName("orderid");
        modelBuilder.Entity<Order>().Property(x => x.Date).HasColumnName("orderdate");
        modelBuilder.Entity<Order>().Property(x => x.ShipName).HasColumnName("shipname");
        modelBuilder.Entity<Order>().Property(x => x.ShipCity).HasColumnName("shipcity");
        modelBuilder.Entity<Order>().Property(x => x.Required).HasColumnName("requireddate");

        //OrderDetails
        modelBuilder.Entity<OrderDetails>().ToTable("orderdetails");
        modelBuilder.Entity<OrderDetails>().HasKey(od => new { od.OrderId, od.ProductId });
        modelBuilder.Entity<OrderDetails>().Property(od => od.OrderId).HasColumnName("orderid");
        modelBuilder.Entity<OrderDetails>().Property(od => od.ProductId).HasColumnName("productid");
        modelBuilder.Entity<OrderDetails>().Property(od => od.UnitPrice).HasColumnName("unitprice");
        modelBuilder.Entity<OrderDetails>().Property(od => od.Discount).HasColumnName("discount");
        modelBuilder.Entity<OrderDetails>().Property(od => od.Quantity).HasColumnName("quantity");




    }
}