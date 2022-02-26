using EAuction.Buyer.Core.Domain;
using EAuction.Persistence.Repositories;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace EAuction.Buyer.Core.Repositories
{
    internal class BuyerRepository : Repository<AuctionBuyer, string>
    {
        public BuyerRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
        }

        public override async Task<AuctionBuyer> findByAsync(string key)
        {
            var filter = Builders<AuctionBuyer>.Filter.Eq(i => i.Phone, key);
            var result = await this.collection.FindAsync<AuctionBuyer>(filter);
            return result.FirstOrDefault();
        }
    }
}