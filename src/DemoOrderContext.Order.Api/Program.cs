using DemoOrderContext.Shared.Contracts;
using DemoOrderContext.Shared.Data;
using DemoOrderContext.Shared.Entities;
using DemoOrderContext.Shared.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(config=>
{
    var connString = builder.Configuration.GetConnectionString("DemoOrderContext");
    config.UseMySql(connString, ServerVersion.AutoDetect(connString));
});

builder.Services.AddTransient<IOrderRepository, OrderRepository>();

builder.Services.AddMassTransit((config) =>
{
    var connString = builder.Configuration.GetSection("RabbitMq:ConnectionString").Value;

    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(connString);
        cfg.ConfigureEndpoints(ctx);
    });
});

var app = builder.Build();

app.MapGet("/", () => "DemoOrderContext.Order.Api running");

app.MapGet("/orders/{id:int}", async (int id, IOrderRepository orderRepository) => 
    await orderRepository.GetOrderByIdAsync(id) 
        is Order order ? Results.Ok(order) : Results.NotFound()
    );

app.MapPost("/orders", async ([FromBody]OrderRequest request, IOrderRepository orderRepository, IPublishEndpoint bus) =>
{
    var order = new Order(request.ProductId, request.Quantity, request.Price);
    await orderRepository.AddOrderAsync(order);

    await bus.Publish(new OrderMessage(order.Id));
    return Results.Created($"/orders/{order.Id}", order);
});

await app.RunAsync();

internal record OrderRequest(int ProductId, int Quantity, decimal Price);