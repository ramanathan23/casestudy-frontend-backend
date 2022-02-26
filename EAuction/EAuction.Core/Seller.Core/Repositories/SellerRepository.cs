using EAuction.Persistence.Repositories;
using EAuction.Seller.Core.Domain;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace EAuction.Seller.Core.Repositories
{
    internal class SellerRepository : Repository<AuctionProductSeller, string>
    {
        public SellerRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
        }

        public override async Task<AuctionProductSeller> findByAsync(string key)
        {
            var filter = Builders<AuctionProductSeller>.Filter.Eq(i => i.Phone, key);
            var result = await this.collection.FindAsync<AuctionProductSeller>(filter);
            return result.FirstOrDefault();
        }
    }
}