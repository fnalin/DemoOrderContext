using DemoOrderContext.Shared.Contracts;
using DemoOrderContext.Shared.Entities;
using DemoOrderContext.Shared.Models;

namespace DemoOrderContext.Shared.Data;

public class OrderRepository (AppDbContext ctx) : IOrderRepository
{
    
    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        var data = await ctx.Orders.FindAsync(id);
        return data;
    }

    public async Task AddOrderAsync(Order order)
    {
        await ctx.Orders.AddAsync(order);
        await ctx.SaveChangesAsync();
    }

    public async Task UpdateStatusOrderAsync(Order order)
    {
        order.Status = StatusOrder.Processed;
        await ctx.SaveChangesAsync();
    }
}