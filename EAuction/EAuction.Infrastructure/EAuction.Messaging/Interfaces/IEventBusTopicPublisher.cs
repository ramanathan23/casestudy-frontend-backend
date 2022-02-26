namespace EAuction.Messaging
{
    public interface IEventBusTopicPublisher : IEventPublisher
    {
        string TopicName
        {
            get;
        }
    }
}