namespace EAuction.Messaging
{
    public interface IEventBusQueueConsumer : IEventConsumer
    {
        string QueueName { get; }
    }
}