using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

namespace EAuction.Messaging
{
    internal class EventBusSubscriber : IEventBusSubscriber
    {
        private readonly ServiceBusProcessor processor;

        public EventBusSubscriber(ServiceBusClient client, string topicName, string subscriberName)
        {
            this.SubscriberName = subscriberName;
            this.processor = client.CreateProcessor(topicName, subscriberName);
            this.processor.ProcessMessageAsync += this.MessageHandler;
            this.processor.ProcessErrorAsync += this.ErrorHandler;
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            if (this.Consume != null) await this.Consume(body);
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            return Task.CompletedTask;
        }

        public string SubscriberName { get; private set; }

        public event Func<string, Task> Consume;

        public async Task StartProcessingAsync()
        {
            await processor.StartProcessingAsync();
        }
    }
}