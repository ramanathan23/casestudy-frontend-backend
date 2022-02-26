using Azure.Messaging.ServiceBus;
using EAuction.Infrastructure.Common;
using System.Threading.Tasks;

namespace EAuction.Messaging
{
    internal class EventBusTopicPublisher : IEventBusTopicPublisher
    {
        private readonly ServiceBusSender sender;

        public EventBusTopicPublisher(ServiceBusClient client, string topicName)
        {
            this.TopicName = topicName;
            this.sender = client.CreateSender(topicName);
        }

        public string TopicName { get; private set; }

        public async Task PublishMessageAsync(EventMessage messageToPublish)
        {
            await this.sender.SendMessageAsync(new ServiceBusMessage(messageToPublish.Message) { Subject = messageToPublish.MessageType });
        }
    }
}