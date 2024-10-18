namespace DataLayer;

public class OrderDetailWithOrderDTO
{
    public int OrderId { get; set; }
    public int UnitPrice { get; set; }
    public int Quantity { get; set; }
    public ProductDetailDTO Product { get; set; }
    public Order Order { get; set; }   
}