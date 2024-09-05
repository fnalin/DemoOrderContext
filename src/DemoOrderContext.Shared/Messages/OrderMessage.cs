namespace DemoOrderContext.Shared.Messages;

public class OrderMessage (int orderId)
{
    public int OrderId { get; set; } = orderId;
}