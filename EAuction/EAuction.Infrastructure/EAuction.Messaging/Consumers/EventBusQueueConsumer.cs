using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

namespace EAuction.Messaging
{
    internal class EventBusQueueConsumer : IEventBusQueueConsumer
    {
        private readonly ServiceBusProcessor processor;

        public EventBusQueueConsumer(ServiceBusClient client, string queueName)
        {
            this.QueueName = queueName;
            processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());
            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;
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

        public async Task StartProcessingAsync()
        {
            await processor.StartProcessingAsync();
        }

        public string QueueName { get; private set; }

        public event Func<string, Task> Consume;
    }
}