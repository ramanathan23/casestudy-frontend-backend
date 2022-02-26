using EAuction.Persistence.Repositories;
using EAuction.Seller.Core.Domain;
using MongoDB.Driver;

namespace EAuction.Seller.Core.Repositories
{
    internal class ProductRepository : Repository<AuctionProduct, string>
    {
        public ProductRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
        }
    }
}