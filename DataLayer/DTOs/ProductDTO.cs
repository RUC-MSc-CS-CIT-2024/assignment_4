namespace DataLayer
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public string ProductName => Name; 
        public string CategoryName { get; set; }
    }
}