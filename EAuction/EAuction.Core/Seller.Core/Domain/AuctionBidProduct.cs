using EAuction.Persistence.Entities;

namespace EAuction.Seller.Core.Domain
{
    internal class AuctionBidProduct : IEntity<string>
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
    }
}