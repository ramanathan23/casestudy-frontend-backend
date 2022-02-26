using EAuction.Infrastructure.Common;
using EAuction.Messaging;
using EAuction.Persistence.Repositories;
using EAuction.Seller.Core.Domain;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAuction.Seller.Core.Services
{
    internal class SellerService : ISellerService
    {
        private readonly IRepository<AuctionProduct, string> productRepository;
        private readonly IRepository<AuctionProductSeller, string> sellerRepository;
        private readonly IEventBusTopicPublisher eventBusTopicPublisher;
        private readonly ILogger<SellerService> logger;

        public SellerService(ILogger<SellerService> logger, IRepository<AuctionProduct, string> productRepository, IRepository<AuctionProductSeller, string> sellerRepository, IEnumerable<IEventBusTopicPublisher> eventQueuePublisherList)
        {
            this.logger = logger;
            this.productRepository = productRepository;
            this.sellerRepository = sellerRepository;
            this.eventBusTopicPublisher = eventQueuePublisherList.FirstOrDefault(i => i.TopicName.ToLower().Equals("eauctionmanagement", StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<bool> AddProductAsync(AuctionProduct auctionProduct, AuctionProductSeller auctionProductSeller)
        {
            try
            {
                AuctionProductSeller seller = await this.sellerRepository.findByAsync(auctionProductSeller.Phone);
                if (seller == null)
                    seller = await this.sellerRepository.AddAsync(auctionProductSeller);

                auctionProduct.SellerId = auctionProductSeller.Id = seller.Id;
                await this.productRepository.AddAsync(auctionProduct);
                await this.eventBusTopicPublisher.PublishMessageAsync(new EventMessage()
                {
                    MessageType = "ProductAdded",
                    Message = JsonConvert.SerializeObject(auctionProduct),
                });
            }
            catch (Exception ex)
            {
                //Log the Expception
                this.logger.LogError($"SellerService - AddProductAsync - {ex.Message}");
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteProductFromAuctionAsync(string productId)
        {
            try
            {
                //publish a delete request to queue
                var result = await this.productRepository.findByAsync(productId);
                if (result != null && result.BidEndDate > DateTime.Now.Date)
                    await this.eventBusTopicPublisher.PublishMessageAsync(new EventMessage()
                    {
                        MessageType = "ProductDeleteRequest",
                        Message = JsonConvert.SerializeObject(result),
                    });
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"SellerService - DeleteProductFromAuctionAsync - {ex.Message}");
                return false;
            }
        }
    }
}