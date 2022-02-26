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
    internal class DeleteProductConfirmSubscriber : IConsumerHandler
    {
        private readonly IServiceScope scope;
        private readonly IEventBusSubscriber consumer;
        private readonly ILogger<DeleteProductConfirmSubscriber> logger;

        public DeleteProductConfirmSubscriber(ILogger<DeleteProductConfirmSubscriber> logger, IServiceProvider serviceProvider, IEnumerable<IEventBusSubscriber> consumers)
        {
            this.logger = logger;
            this.scope = serviceProvider.CreateScope();
            this.consumer = consumers.FirstOrDefault(i => i.SubscriberName.ToLower().Equals("ProductDeleteConfirmation", StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task HandleMessageAsync(string message)
        {
            try
            {
                var product = JsonConvert.DeserializeObject<AuctionProduct>(message);
                if (product != null) await this.scope.ServiceProvider.GetRequiredService<IRepository<AuctionProduct, string>>().DeleteAsync(product.Id);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Consumer - Delete Product - {ex.Message}");
            }
        }

        public async Task RegisterAsync()
        {
            this.consumer.Consume += this.HandleMessageAsync;
            await this.consumer.StartProcessingAsync();
        }
    }
}