namespace EAuction.Messaging
{
    public interface IEventBusQueuePublisher : IEventPublisher
    {
        string QueueName { get; }
    }
}