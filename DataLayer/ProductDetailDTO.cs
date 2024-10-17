namespace DataLayer;

public class ProductDetailDTO
{
    public string Name { get; set; }
    public string ProductName => Name;
    public CategoryDTO Category { get; set; }
}