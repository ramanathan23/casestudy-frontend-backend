using EAuction.Persistence.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EAuction.Buyer.Core.Domain
{
    public class AuctionBid : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ProductId { get; set; }
        public string BuyerId { get; set; }
        public decimal BidAmount { get; set; }
    }
}