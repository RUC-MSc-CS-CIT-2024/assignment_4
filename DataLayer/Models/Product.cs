using System.Globalization;

namespace DataLayer;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? QuantityPerUnit { get; set; }
    public int UnitsInStock { get; set; }
    public int UnitPrice { get; set; }
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; }
    public string CategoryName => Category?.Name;
}

public class ProductCategoryName
{
    public string CategoryName;
    public string ProductName;
}