using EAuction.Buyer.Core.Domain;
using EAuction.Persistence.Repositories;
using System.Threading.Tasks;

namespace EAuction.Buyer.Core.Repositories
{
    public interface IBidRepository : IRepository<AuctionBid, string>
    {
        Task<AuctionBid> FindBidByAsync(string productId, string buyerId);
    }
}