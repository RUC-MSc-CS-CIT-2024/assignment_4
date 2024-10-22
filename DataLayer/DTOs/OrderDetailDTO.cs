namespace DataLayer
{
    public class OrderDetailDTO
    {
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public ProductDetailDTO Product { get; set; }
    }
}