using EAuction.BidListing.Core.Domain;
using System.Threading.Tasks;

namespace EAuction.BidListing.Core.Services
{
    public interface IBidListingService
    {
        Task AddProduct(ProductAndBidDetails productAndBidDetails);

        Task AddOrUpdateBids(ProductAndBidDetails productAndBidDetails);

        Task<ProductAndBidDetails> GetProductAndBidDetails(string productId);
    }
}