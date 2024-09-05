using DemoOrderContext.Shared.Contracts;
using DemoOrderContext.Shared.Messages;
using MassTransit;

namespace DemoOrderContext.Order.Worker;

public class OrderQueueConsumer (ILogger<OrderQueueConsumer> logger, IOrderRepository orderRepository) 
    : IConsumer<OrderMessage>
{
    public async Task Consume(ConsumeContext<OrderMessage> context)
    {
        var orderId = context.Message.OrderId;
        logger.LogInformation("Order received: {Id}", orderId);

        var order = await orderRepository.GetOrderByIdAsync(orderId);
        logger.LogInformation("Order : {order} - processing", order);

        // throw new Exception("Deu ruim nesse pedido");
        await orderRepository.UpdateStatusOrderAsync(order);
        
        logger.LogInformation("Order : {Id} - processed", orderId);

    }
}

public class OrderQueueConsumerDefinition : ConsumerDefinition<OrderQueueConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<OrderQueueConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        consumerConfigurator.UseMessageRetry(r=> r.Interval(3, TimeSpan.FromSeconds(5)));
    }
}