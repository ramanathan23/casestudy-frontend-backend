namespace EAuction.Messaging
{
    public interface IEventBusSubscriber : IEventConsumer
    {
        string SubscriberName { get; }
    }
}