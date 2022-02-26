using EAuction.Buyer.Core.Domain;
using EAuction.Buyer.Core.Domain.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAuction.Buyer.Core.Services
{
    public interface IBuyerService
    {
        Task<bool> AddBidAsync(AuctionBuyer auctionBuyer, AuctionBid auctionBid);

        Task<bool> UpdateBidAsync(string phoneNumber, string productId, decimal bidAmount);

        Task<IList<AuctionBuyerBidDetails>> GetBidsForProduct(string productId);
    }
}