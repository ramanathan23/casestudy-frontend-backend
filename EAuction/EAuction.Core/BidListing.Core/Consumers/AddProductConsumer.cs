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
    internal class AddProductConsumer : IConsumerHandler
    {
        private readonly IServiceScope scope;
        private readonly IEventBusSubscriber consumer;
        private readonly ILogger<AddProductConsumer> logger;

        public AddProductConsumer(ILogger<AddProductConsumer> logger, IServiceProvider serviceProvider, IEnumerable<IEventBusSubscriber> consumers)
        {
            this.scope = serviceProvider.CreateScope();
            this.consumer = consumers.FirstOrDefault(i => i.SubscriberName.ToLower().Equals("ProductAdded", StringComparison.InvariantCultureIgnoreCase));
            this.logger = logger;
        }

        public async Task HandleMessageAsync(string message)
        {
            try
            {
                var product = JsonConvert.DeserializeObject<AuctionProduct>(message);
                if (product != null)
                {
                    var bidListingService = this.scope.ServiceProvider.GetRequiredService<IBidListingService>();
                    await bidListingService.AddProduct(new ProductAndBidDetails()
                    {
                        BidEndDate = product.BidEndDate,
                        Category = product.Category,
                        Id = product.Id,
                        ProductName = product.ProductName,
                        ShortDescription = product.ShortDescription,
                        StartingPrice = product.StartingPrice
                    });
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Consumer - ProductAdded - {ex.Message}");
            }
        }

        public async Task RegisterAsync()
        {
            this.consumer.Consume += this.HandleMessageAsync;
            await this.consumer.StartProcessingAsync();
        }
    }
}