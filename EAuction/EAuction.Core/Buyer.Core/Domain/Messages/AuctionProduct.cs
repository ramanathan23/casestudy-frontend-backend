using EAuction.Persistence.Entities;

namespace EAuction.Buyer.Core.Domain.Messages
{
    internal class AuctionProduct : IEntity<string>
    {
        public string Id { get; set; }
    }
}