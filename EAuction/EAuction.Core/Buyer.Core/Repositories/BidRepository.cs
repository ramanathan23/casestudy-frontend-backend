using EAuction.Buyer.Core.Domain;
using EAuction.Persistence.Repositories;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace EAuction.Buyer.Core.Repositories
{
    internal class BidRepository : Repository<AuctionBid, string>, IBidRepository
    {
        public BidRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
        }

        public async Task<AuctionBid> FindBidByAsync(string productId, string buyerId)
        {
            var filter = Builders<AuctionBid>.Filter.Eq(i => i.ProductId, productId);
            filter &= Builders<AuctionBid>.Filter.Eq(i => i.BuyerId, buyerId);
            var result = await this.collection.FindAsync<AuctionBid>(filter);
            return result.FirstOrDefault();
        }
    }
}