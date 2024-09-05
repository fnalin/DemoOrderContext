using DemoOrderContext.Shared.Entities;

namespace DemoOrderContext.Shared.Contracts;

public interface IOrderRepository
{
    Task<Order?> GetOrderByIdAsync(int id);
    Task AddOrderAsync(Order order);
    
    Task UpdateStatusOrderAsync(Order order);
}