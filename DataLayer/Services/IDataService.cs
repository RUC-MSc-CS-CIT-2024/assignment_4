namespace DataLayer;

public interface IDataService
{
    //Category
    IList<Category> GetCategories();
    Category GetCategory(int id);
    Category CreateCategory(string name, string description);
    bool UpdateCategory(int id, string name, string description);
    bool DeleteCategory(int id);

    //Product
    ProductDTO GetProduct(int id);
    IList<ProductDTO> GetProductByCategory(int id);
    IList<ProductDTO> GetProductByName(string name);

    //Order
    OrderDTO GetOrder(int id);
    IList<OrderDTO> GetOrders();

    IList<OrderDetailDTO> OrderDetails(int id);
    IList<OrderDetailDTO> GetOrderDetailsByOrderId(int id);
    IList<OrderDetailWithOrderDTO> GetOrderDetailsByProductId(int id);
}