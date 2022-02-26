using EAuction.Seller.Core.Domain;
using System.Threading.Tasks;

namespace EAuction.Seller.Core.Services
{
    public interface ISellerService
    {
        Task<bool> AddProductAsync(AuctionProduct auctionProduct, AuctionProductSeller productSeller);

        Task<bool> DeleteProductFromAuctionAsync(string productId);
    }
}