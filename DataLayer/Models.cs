using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer;

[Table("orders")]
public class Order
{
    [Key, Column("orderid")]
    public int Id { get; set; }
    [Column("orderdate")]
    public DateTime Date { get; set; }
    [Column("requireddate")]
    public DateTime Required { get; set; }
    [Column("shippeddate")]
    public DateTime? Shipped { get; set; }
    [Column("freight")]
    public int? Freight { get; set; }
    [Column("shipname")]
    public string? ShipName { get; set; }
    [Column("shipcity")]
    public string? ShipCity { get; set; }

    public List<OrderDetails> OrderDetails { get; set; }
}

[Table("orderdetails")]
public class OrderDetails
{
    [Column("unitprice")]
    public int UnitPrice { get; set; }
    [Column("quantity")]
    public int Quantity { get; set; }
    [Column("discount")]
    public int Discount { get; set; }

    [Column("productid")]
    public int ProductId { get; set; }
    [Column("orderid")]
    public int OrderId { get; set; }

    public Product Product { get; set; }
    public Order Order { get; set; }
}

[Table("products")]
public class Product
{
    [Key, Column("productid")]
    public int Id { get; set; }
    [Column("productname")]
    public string Name { get; set; }
    [Column("unitprice")]
    public double UnitPrice { get; set; }
    [Column("quantityperunit")]
    public string? QuantityPerUnit { get; set; }
    [Column("unitsinstock")]
    public int UnitsInStock { get; set; }

    [Column("categoryid")]
    public int? CategoryId { get; set; }

    public Category? Category { get; set; }
    public List<OrderDetails> OrderDetails { get; set; }

    public string CategoryName 
    { 
        get => Category?.Name ?? "";
    }
}

[Table("categories")]
public class Category
{
    [Key, Column("categoryid")]
    public int Id { get; set; }
    [Column("categoryname")]
    public string Name { get; set; }
    [Column("description")]
    public string? Description { get; set; }

    public List<Product> Products { get; set; }
}

public class SimpleProductDto {
    public string ProductName { get; set; }
    public string CategoryName { get; set; }
}
