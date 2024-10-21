using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataLayer;

public interface IDataService {
    Category CreateCategory(string name, string description);
    bool DeleteCategory(int categoryId);
    List<Category> GetCategories();
    Category? GetCategory(int categoryId);
    Order GetOrder(int orderId);
    List<OrderDetails> GetOrderDetailsByOrderId(int orderId);
    List<OrderDetails> GetOrderDetailsByProductId(int productId);
    List<Order> GetOrders(string shippingName);
    List<Order> GetOrders();
    Product? GetProduct(int productId);
    List<Product> GetProductByCategory(int categoryId);
    List<SimpleProductDto> GetProductByName(string productNameSubstring);
    bool UpdateCategory(int categoryId, string name, string description);
}

public class DataService: IDataService
{
    private readonly NorthwindContext _context;

    public DataService() { 
        _context = new NorthwindContext();
    }

    [ActivatorUtilitiesConstructor]
    public DataService(NorthwindContext context) {
        _context = context;
    }

    public Category CreateCategory(string name, string description)
    {
        int maxId = _context.Categories.Max(c => c.Id);
        Category newCat = new() { Id = maxId + 1, Name = name, Description = description };
        _context.Categories.Add(newCat);
        _context.SaveChanges();

        return newCat;
    }

    public bool DeleteCategory(int categoryId)
    {
        Category? cat = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
        if (cat == null)
            return false;
        _context.Categories.Remove(cat);
        _context.SaveChanges();
        return true;
    }

    public List<Category> GetCategories() 
        => _context.Categories.ToList();

    public Category? GetCategory(int categoryId) 
        => _context.Categories.FirstOrDefault(c => c.Id == categoryId);

    public Order GetOrder(int orderId) 
        => _context.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
            .ThenInclude(p => p.Category)
            .First(o => o.Id == orderId);

    public List<OrderDetails> GetOrderDetailsByOrderId(int orderId) 
        => _context.OrderDetails
            .Include(od => od.Product)
            .Where(od => od.OrderId == orderId)
            .ToList();

    public List<OrderDetails> GetOrderDetailsByProductId(int productId) 
        => _context.OrderDetails
            .Include(od => od.Order)
            .Include(od => od.Product)
            .Where(od => od.ProductId == productId)
            .OrderBy(od => od.OrderId)
            .ToList();

    public List<Order> GetOrders(string shippingName) 
        => _context.Orders
            .Where(o => o.ShipName == shippingName)
            .ToList();

    public List<Order> GetOrders() 
        => _context.Orders
            .ToList();

    public Product? GetProduct(int productId) 
        => _context.Products
            .Include(p => p.Category)
            .FirstOrDefault(p => p.Id == productId);

    public List<Product> GetProductByCategory(int categoryId) 
        => _context.Products
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId)
            .ToList();

    public List<SimpleProductDto> GetProductByName(string productNameSubstring) 
        => _context.Products
            .Include(p => p.Category)
            .Where(p => p.Name.Contains(productNameSubstring))
            .Select(p => new SimpleProductDto() { ProductName = p.Name, CategoryName = p.Category.Name })
            .ToList();

    public bool UpdateCategory(int categoryId, string name, string description)
    {
        Category? cat = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
        if (cat == null)
            return false;

        cat.Name = name;
        cat.Description = description;
        _context.SaveChanges();
        return true;
    }
}
