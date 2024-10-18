namespace DataLayer;

public class OrderDTO
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public IList<OrderDetailDTO> OrderDetails { get; set; } = new List<OrderDetailDTO>();
}
