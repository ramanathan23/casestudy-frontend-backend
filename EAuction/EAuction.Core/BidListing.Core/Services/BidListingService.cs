using EAuction.BidListing.Core.Domain;
using EAuction.Persistence.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EAuction.BidListing.Core.Services
{
    internal class BidListingService : IBidListingService
    {
        private readonly IRepository<ProductAndBidDetails, string> repository;
        private readonly ILogger<BidListingService> logger;

        public BidListingService(IRepository<ProductAndBidDetails, string> repository, ILogger<BidListingService> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task AddOrUpdateBids(ProductAndBidDetails productAndBidDetails)
        {
            try
            {
                var entity = await this.repository.findByAsync(productAndBidDetails.Id);
                entity.Bids = productAndBidDetails.Bids;
                await this.repository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"BidListingService - AddOrUpdateBids - {ex.Message}");
            }
        }

        public async Task AddProduct(ProductAndBidDetails productAndBidDetails)
        {
            try
            {
                await this.repository.AddAsync(productAndBidDetails);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"BidListingService - AddProduct - {ex.Message}");
            }
        }

        public async Task<ProductAndBidDetails> GetProductAndBidDetails(string productId)
        {
            try
            {
                return await this.repository.findByAsync(productId);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"BidListingService - GetProductAndBidDetails - {ex.Message}");
            }
            return null;
        }
    }
}