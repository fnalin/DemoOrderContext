
using DemoOrderContext.Order.Worker;
using DemoOrderContext.Shared.Contracts;
using DemoOrderContext.Shared.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>(config=>
{
    var connString = builder.Configuration.GetConnectionString("DemoOrderContext");
    config.UseMySql(connString, ServerVersion.AutoDetect(connString));
});

builder.Services.AddTransient<IOrderRepository, OrderRepository>();



builder.Services.AddMassTransit((config) =>
{
    config.AddConsumer<OrderQueueConsumer, OrderQueueConsumerDefinition>();
    
    var connString = builder.Configuration.GetSection("RabbitMq:ConnectionString").Value;
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(connString);
        
        cfg.ReceiveEndpoint("order-queue", e =>
        {
            e.ConfigureConsumer<OrderQueueConsumer>(ctx);
        });
        
        cfg.ConfigureEndpoints(ctx);
    });
    
});

var host = builder.Build();
await host.RunAsync();