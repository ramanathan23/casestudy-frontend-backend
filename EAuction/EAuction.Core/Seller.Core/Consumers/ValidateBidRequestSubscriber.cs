using EAuction.Infrastructure.Common;
using EAuction.Messaging;
using EAuction.Persistence.Repositories;
using EAuction.Seller.Core.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAuction.Seller.Core.Consumers
{
    internal class ValidateBidRequestSubscriber : IConsumerHandler
    {
        private readonly IServiceScope scope;
        private readonly IEventBusSubscriber consumer;
        private readonly IEventBusTopicPublisher publisher;
        private readonly ILogger<ValidateBidRequestSubscriber> logger;

        public ValidateBidRequestSubscriber(ILogger<ValidateBidRequestSubscriber> logger, IServiceProvider serviceProvider, IEnumerable<IEventBusSubscriber> consumers, IEnumerable<IEventBusTopicPublisher> publishers)
        {
            this.logger = logger;
            this.scope = serviceProvider.CreateScope();
            this.consumer = consumers.FirstOrDefault(i => i.SubscriberName.ToLower().Equals("ValidateBidRequest", StringComparison.InvariantCultureIgnoreCase));
            this.publisher = publishers.FirstOrDefault(i => i.TopicName.ToLower().Equals("eauctionmanagement", StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task HandleMessageAsync(string message)
        {
            try
            {
                var product = JsonConvert.DeserializeObject<AuctionBidProduct>(message);
                if (product != null)
                {
                    var result = await this.scope.ServiceProvider.GetRequiredService<IRepository<AuctionProduct, string>>().findByAsync(product.ProductId);
                    if (result != null && result.BidEndDate > DateTime.Now.Date)
                        await this.publisher.PublishMessageAsync(new EventMessage() { MessageType = "AddOrUpdateBidConfirm", Message = message });
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Consumer - ValidateBidRequest - {ex.Message}");
            }
        }

        public async Task RegisterAsync()
        {
            this.consumer.Consume += this.HandleMessageAsync;
            await this.consumer.StartProcessingAsync();
        }
    }
}