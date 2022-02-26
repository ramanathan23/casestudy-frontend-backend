using EAuction.Buyer.Core.Domain;
using EAuction.Buyer.Core.Domain.Messages;
using EAuction.Buyer.Core.Repositories;
using EAuction.Core.Common.Exceptions;
using EAuction.Infrastructure.Common;
using EAuction.Messaging;
using EAuction.Persistence.Repositories;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAuction.Buyer.Core.Services
{
    internal class BuyerService : IBuyerService
    {
        private readonly IRepository<AuctionBuyer, string> buyerRepository;
        private readonly IBidRepository bidRepository;
        private readonly IEventBusTopicPublisher eventBusTopicPublisher;
        private readonly ILogger<BuyerService> logger;

        public BuyerService(ILogger<BuyerService> logger, IRepository<AuctionBuyer, string> buyerRepository, IBidRepository bidRepository, IEnumerable<IEventBusTopicPublisher> publishers)
        {
            this.logger = logger;
            this.buyerRepository = buyerRepository;
            this.bidRepository = bidRepository;
            this.eventBusTopicPublisher = publishers.FirstOrDefault(i => i.TopicName.ToLower().Equals("eauctionmanagement", StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<bool> AddBidAsync(AuctionBuyer auctionBuyer, AuctionBid auctionBid)
        {
            try
            {

                AuctionBuyer buyer = await this.FindOrAddBuyer(auctionBuyer);
                var bid = await this.bidRepository.FindBidByAsync(auctionBid.ProductId, buyer.Id);

                if (bid != null)
                    throw new EAuctionDomainException($"Bid already exists - productId: {auctionBid.ProductId}, buyerPhone: {auctionBuyer.Phone}");

                auctionBid.BuyerId = buyer.Id;
                await this.eventBusTopicPublisher.PublishMessageAsync(new EventMessage()
                {
                    MessageType = "AddOrUpdateBid",
                    Message = JsonConvert.SerializeObject(auctionBid)
                });
            }
            catch (Exception ex)
            {
                this.logger.LogError($"BuyerService - AddBidAsync - {ex.Message}");
                return false;
            }
            return true;
        }

        private async Task<AuctionBuyer> FindOrAddBuyer(AuctionBuyer auctionBuyer)
        {
            var result = await this.buyerRepository.findByAsync(auctionBuyer.Phone);
            if (result == null) result = await this.buyerRepository.AddAsync(auctionBuyer);
            return result;
        }

        public async Task<bool> UpdateBidAsync(string phoneNumber, string productId, decimal bidAmount)
        {
            try
            {
                var auctionBuyer = await this.buyerRepository.findByAsync(phoneNumber);

                if (auctionBuyer == null)
                        return false;

                await this.eventBusTopicPublisher.PublishMessageAsync(new EventMessage()
                {
                    MessageType = "AddOrUpdateBid",
                    Message = JsonConvert.SerializeObject(new AuctionBid() { BidAmount = bidAmount, BuyerId = auctionBuyer.Id, ProductId = productId })
                });
            }
            catch (Exception ex)
            {
                this.logger.LogError($"BuyerService - UpdateBidAsync - {ex.Message}");
                return false;
            }
            return true;
        }

        public async Task<IList<AuctionBuyerBidDetails>> GetBidsForProduct(string productId)
        {
            try
            {
                //using AsEnumreable is pretty bad idea but with actual mongo join query is not working.
                //aggregate([{ "$lookup" : { "from" : "AuctionBuyer", "localField" : "BuyerId", "foreignField" : "_id", "as" : "r" } }, { "$project" : { "r" : "$r", "_id" : 0 } }])
                return await Task.Run(() =>
                {
                    return this.bidRepository.Query().AsEnumerable()
                    .Join(this.buyerRepository.Query().AsEnumerable(), i => i.BuyerId, k => k.Id,
                          (bid, buyer) => new AuctionBuyerBidDetails() {
                                               ProductId = bid.ProductId,
                                               BidAmount = bid.BidAmount,
                                               Email = buyer.Email,
                                               FirstName = buyer.FirstName,
                                               Phone = buyer.Phone
                          }).Where(i => i.ProductId == productId).OrderByDescending(i => i.BidAmount).ToList();
                });
            }
            catch (Exception ex)
            {
                this.logger.LogError($"BuyerService - GetBidsForProduct - {ex.Message}");
                return null;
            }
        }
    }
}