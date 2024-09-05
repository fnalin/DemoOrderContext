using DemoOrderContext.Shared.Models;

namespace DemoOrderContext.Shared.Entities;

public class Order
{
    public Order(int productId, int quantity, decimal price)
    {
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }

    public Order()
    {
        
    }

    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public StatusOrder Status { get; set; } = StatusOrder.Received;
}