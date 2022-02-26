using EAuction.BidListing.Core.Domain;
using EAuction.BidListing.Core.Domain.Messages;
using EAuction.BidListing.Core.Services;
using EAuction.Infrastructure.Common;
using EAuction.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAuction.BidListing.Core.Consumers
{
    internal class BidAddedOrUpdatedConsumer : IConsumerHandler
    {
        private readonly IServiceScope scope;
        private readonly IEventBusSubscriber consumer;
        private readonly ILogger<BidAddedOrUpdatedConsumer> logger;

        public BidAddedOrUpdatedConsumer(ILogger<BidAddedOrUpdatedConsumer> logger, IServiceProvider serviceProvider, IEnumerable<IEventBusSubscriber> consumers)
        {
            this.logger = logger;
            this.scope = serviceProvider.CreateScope();
            this.consumer = consumers.FirstOrDefault(i => i.SubscriberName.ToLower().Equals("BidAddedOrUpdated", StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task HandleMessageAsync(string message)
        {
            try
            {
                var bidAddOrUpdateMessage = JsonConvert.DeserializeObject<BidAddOrUpdateMessage>(message);
                if (bidAddOrUpdateMessage != null)
                {
                    var bidListingService = this.scope.ServiceProvider.GetRequiredService<IBidListingService>();
                    await bidListingService.AddOrUpdateBids(new ProductAndBidDetails()
                    {
                        Id = bidAddOrUpdateMessage.ProductId,
                        Bids = bidAddOrUpdateMessage.AuctionBuyerBidDetails.Select(i =>
                                new BidDetails()
                                {
                                    BidAmount = i.BidAmount,
                                    Email = i.Email,
                                    FirstName = i.FirstName,
                                    Phone = i.Phone
                                }).ToList()
                    });
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Consumer - BidAddedOrUpdated - {ex.Message}");
            }
        }

        public async Task RegisterAsync()
        {
            this.consumer.Consume += this.HandleMessageAsync;
            await this.consumer.StartProcessingAsync();
        }
    }
}