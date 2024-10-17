using Microsoft.EntityFrameworkCore;
namespace DataLayer;

public class DataService
{
    public IList<Category> GetCategories()
    {
        var db = new NorthwindContext();
        return db.Categories.ToList();
    }

    public Category? GetCategory(int id)
    {
        var db = new NorthwindContext();
        
        return db.Categories.Find(id);
    }

    public Category CreateCategory(String name, String description)
    {
        var db = new NorthwindContext();

        int id = db.Categories.Max(x => x.Id) + 1; // Calculate a new ID
        var category = new Category
        {
            Id = id,
            Name = name,
            Description = description
        };

        db.Categories.Add(category);
        db.SaveChanges();
        return category;
    }
    
    public bool DeleteCategory(int id)
    {
        var db = new NorthwindContext();
        
        var category = db.Categories.Find(id);

        if (db.Categories.Find(id) == null)
        {
            return false;
        }
        
        db.Categories.Remove(category);
        return db.SaveChanges() > 0;
    }
    
    public int AddCategory(string name, string description)
    {
        var db = new NorthwindContext();
        
        int id = db.Categories.Max(x => x.Id) + 1;
        var category = new Category
        {
            Name = name,
            Id = id,
            Description = description
        };
        
        db.Categories.Add(category);
        
        db.SaveChanges();

        return category.Id;
    }

    public bool UpdateCategory(int id, string name, string description)
    {
        using (var db = new NorthwindContext())
        {
            var oldCategory = db.Categories.Find(id);

            if (oldCategory == null)
            {
                return false;
            }
            
            oldCategory.Name = name;
            oldCategory.Description = description;
           
            return db.SaveChanges() > 0;
        }
    }
    

    public Product? GetProduct(int id)
    {
        using (var db = new NorthwindContext())
        {
            var product = db.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);

            return product;
        }
    }

    public IList<Product> GetProductByCategory(int categoryId)
    {
        using (var db = new NorthwindContext())
        {
            return db.Products
                .Include(p => p.Category) 
                .Where(p => p.CategoryId == categoryId)
                .ToList();
        }
    }

    public IList<ProductCategoryName> GetProductByName(string text)
    {
        using (var db = new NorthwindContext())
        {
            var results = db.Products
                .Include(p => p.Category) // Eager load the related Category
                .Where(p => p.Name.Contains(text)) // Filter by substring
                .Select(p => new ProductCategoryName
                {
                    ProductName = p.Name,
                    CategoryName = p.Category.Name
                })
                .ToList();

            return results;
        }
    }

    public Order GetOrder(int id)
    {
        using (var db = new NorthwindContext())
        {
            var order = db.Orders.Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.Category)
                .FirstOrDefault(o => o.Id == id); // Fetch the order by its ID

            return order;
        }
    }

    public IList<Order> GetOrders()
    {
        using (var db = new NorthwindContext())
        {
            var orders = db.Orders.ToList();
            
            return orders;
        }
    }

    public IList<OrderDetails> GetOrderDetailsByOrderId(int orderId)
    {
        using (var db = new NorthwindContext())
        {
            var details = db.OrderDetails.Include(od => od.Product)
                .Where(x => x.OrderId == orderId).ToList();
            
            return details;
        }
    }
    
    public IList<OrderDetails> GetOrderDetailsByProductId(int productId)
    {
        using (var db = new NorthwindContext())
        {
            var orderDetails = db.OrderDetails
                .Where(od => od.ProductId == productId) // Filter by Product ID
                .Include(od => od.Product) // Include the Product
                .Include(od => od.Order)   // Include the Order
                .OrderBy(od => od.OrderId) // Order by OrderId
                .ToList();
            return orderDetails;
        }
    }

}
