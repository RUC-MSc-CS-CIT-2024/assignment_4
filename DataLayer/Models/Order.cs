namespace DataLayer;

public class Order
{
    public int Id { get; set; } = 0;
    public DateTime Date { get; set; } = new DateTime();
    public DateTime Required { get; set; } = new DateTime();
    public string ShipName { get; set; } = null;
    public string ShipCity { get; set; } = null;
    public virtual ICollection<OrderDetails> OrderDetails { get; set; } = null;
}
