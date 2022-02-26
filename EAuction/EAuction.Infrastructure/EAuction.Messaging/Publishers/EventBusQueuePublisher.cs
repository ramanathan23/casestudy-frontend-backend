using Azure.Messaging.ServiceBus;
using EAuction.Infrastructure.Common;
using System.Threading.Tasks;

namespace EAuction.Messaging
{
    internal class EventBusQueuePublisher : IEventBusQueuePublisher
    {
        private readonly ServiceBusSender sender;

        public EventBusQueuePublisher(ServiceBusClient client, string queueName)
        {
            this.QueueName = queueName;
            this.sender = client.CreateSender(queueName);
        }

        public string QueueName { get; private set; }

        public async Task PublishMessageAsync(EventMessage messageToPublish)
        {
            await this.sender.SendMessageAsync(new ServiceBusMessage(messageToPublish.Message) { Subject = messageToPublish.MessageType });
        }
    }
}