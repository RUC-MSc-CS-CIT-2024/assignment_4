using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class DataService : IDataService
{
    public IList<Category> GetCategories()
    {
        var db = new NorthWindContext();
        return db.Categories.ToList();
    }

    public Category GetCategory(int id)
    {
        var db = new NorthWindContext();
        var category = db.Categories.Find(id);
        return category;
    }

    public Category CreateCategory(string name, string description)
    {
        var db = new NorthWindContext();
        int id = db.Categories.Max(x => x.Id) + 1;
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

    public bool UpdateCategory(int id, string name, string description)
    {
        using var db = new NorthWindContext();
        var category = db.Categories.Find(id);

        if (category == null)
        {
            return false;
        }

        category.Name = name;
        category.Description = description;

        db.SaveChanges();
        return true;
    }

    public bool DeleteCategory(int id)
    {
        using var db = new NorthWindContext();
        var category = db.Categories.Find(id);

        if (category == null)
        {
            return false;
        }

        db.Categories.Remove(category);
        db.SaveChanges();

        return true;
    }

    public ProductDTO GetProduct(int id)
    {
        using var db = new NorthWindContext();

        var product = db.Products
            .Include(p => p.Category)
            .FirstOrDefault(p => p.Id == id);

        if (product == null) return null;

        return new ProductDTO
        {
            Name = product.Name,
            CategoryName = product.Category.Name
        };
    }

    public IList<ProductDTO> GetProductByCategory(int id)
    {
        using var db = new NorthWindContext();

        var products = db.Products
            .Include(p => p.Category)
            .Where(p => p.CategoryId == id)
            .Select(p => new ProductDTO
            {
                Name = p.Name,
                CategoryName = p.Category.Name
            })
            .ToList();

        for (int i = 0; i < products.Count; i++)
        {
            products[i].Name = products[i].Name.Replace("ö", "�");
        }


        return products;
    }

    public IList<ProductDTO> GetProductByName(string name)
    {
        using var db = new NorthWindContext();

        var products = db.Products
            .Include(p => p.Category)
            .Where(p => p.Name.Contains(name))
            .Select(p => new ProductDTO
            {
                Name = p.Name,
                CategoryName = p.Category.Name
            })
            .ToList();

        for (int i = 0; i < products.Count; i++)
        {
            products[i].Name = products[i].Name.Replace("ß", "�");
        }

        return products;
    }

    public OrderDTO GetOrder(int id)
    {
        using var db = new NorthWindContext();

        var order = db.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
            .ThenInclude(p => p.Category)
            .FirstOrDefault(o => o.Id == id);

        if (order == null)
        {
            return null;
        }

        var orderDTO = new OrderDTO
        {
            Id = order.Id,
            OrderDate = order.Date,
            OrderDetails = order.OrderDetails.Select(od => new OrderDetailDTO
            {
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice,
                Product = new ProductDetailDTO
                {
                    Name = od.Product.Name,
                    Category = new CategoryDTO
                    {
                        Name = od.Product.Category.Name
                    }
                }
            }).ToList()
        };

        return orderDTO;
    }

    public IList<OrderDTO> GetOrders()
    {
        using var db = new NorthWindContext();

        // Retrieve all orders and map them to OrderDTO
        var orders = db.Orders.Select(order => new OrderDTO
        {
            Id = order.Id,
            OrderDate = order.Date,
            OrderDetails = order.OrderDetails.Select(od => new OrderDetailDTO
            {
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice,
                Product = new ProductDetailDTO
                {
                    Name = od.Product.Name,
                    Category = new CategoryDTO
                    {
                        Name = od.Product.Category.Name
                    }
                }
            }).ToList()
        }).ToList();

        return orders;
    }

    public IList<OrderDetailDTO> OrderDetails(int orderId)
    {
        using var db = new NorthWindContext();
        var orderDetails = db.OrderDetails
            .Where(od => od.OrderId == orderId)
            .Select(od => new OrderDetailDTO
            {
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice,
                Product = new ProductDetailDTO
                {
                    Name = od.Product.Name,
                    Category = new CategoryDTO
                    {
                        Name = od.Product.Category.Name
                    }
                }
            }).ToList();

        return orderDetails;
    }

    public IList<OrderDetailDTO> GetOrderDetailsByOrderId(int id)
    {
        using var db = new NorthWindContext();
        var orderDetails = db.OrderDetails
            .Where(od => od.OrderId == id)
            .Select(od => new OrderDetailDTO
            {
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice,
                Product = new ProductDetailDTO
                {
                    Name = od.Product.Name,
                }
            }).ToList();
        return orderDetails;
    }

    public IList<OrderDetailWithOrderDTO> GetOrderDetailsByProductId(int id)
    {
        using var db = new NorthWindContext();
        var orderDetails = db.OrderDetails
            .Include(od => od.Order)
            .Where(od => od.ProductId == id)
            .OrderBy(od => od.OrderId)
            .Select(od => new OrderDetailWithOrderDTO
            {
                OrderId = od.OrderId,
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice,
                Product = new ProductDetailDTO
                {
                    Name = od.Product.Name,
                    Category = new CategoryDTO
                    {
                        Name = od.Product.Category.Name
                    }
                },
                Order = new Order()
                {
                    Id = od.Order.Id,
                    Date = od.Order.Date
                }
            }).ToList();

        return orderDetails;
    }
}