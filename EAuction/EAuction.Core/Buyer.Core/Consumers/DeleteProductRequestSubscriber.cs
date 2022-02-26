using EAuction.Buyer.Core.Domain;
using EAuction.Buyer.Core.Domain.Messages;
using EAuction.Buyer.Core.Repositories;
using EAuction.Infrastructure.Common;
using EAuction.Messaging;
using EAuction.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAuction.Buyer.Core.Consumers
{
    internal class DeleteProductSubscriber : IConsumerHandler
    {
        private readonly IServiceScope scope;
        private readonly IEventBusSubscriber consumer;
        private readonly IEventBusTopicPublisher publisher;
        private readonly ILogger<DeleteProductSubscriber> logger;

        public DeleteProductSubscriber(ILogger<DeleteProductSubscriber> logger, IServiceProvider serviceProvider, IEnumerable<IEventBusSubscriber> consumers, IEnumerable<IEventBusTopicPublisher> publishers)
        {
            this.logger = logger;
            this.scope = serviceProvider.CreateScope();
            this.consumer = consumers.FirstOrDefault(i => i.SubscriberName.ToLower().Equals("ProductDeleteRequest", StringComparison.InvariantCultureIgnoreCase));
            this.publisher = publishers.FirstOrDefault(i => i.TopicName.ToLower().Equals("eauctionmanagement", StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task HandleMessageAsync(string message)
        {
            try
            {
                var product = JsonConvert.DeserializeObject<AuctionProduct>(message);
                if (product != null)
                {
                    var result = this.scope.ServiceProvider.GetRequiredService<IBidRepository>().Query().Where(i => i.ProductId == product.Id).Count();
                    if (result == 0)
                        await this.publisher.PublishMessageAsync(new EventMessage() { MessageType = "ProductDeleteConfirmation", Message = message });
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Consumer - DeleteProduct - {ex.Message}");
            }
        }

        public async Task RegisterAsync()
        {
            this.consumer.Consume += this.HandleMessageAsync;
            await this.consumer.StartProcessingAsync();
        }
    }
}