using EAuction.BidListing.Core.Domain;
using EAuction.Persistence.Repositories;
using MongoDB.Driver;

namespace EAuction.BidListing.Core.Repositories
{
    internal class ProductAndBidDetailsRepository : Repository<ProductAndBidDetails, string>
    {
        public ProductAndBidDetailsRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
        }
    }
}