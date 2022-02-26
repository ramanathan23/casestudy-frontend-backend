using System.Collections.Generic;

namespace EAuction.BidListing.Core.Domain.Messages
{
    public class BidAddOrUpdateMessage
    {
        public string ProductId { get; set; }
        public IList<AuctionBuyerBidDetails> AuctionBuyerBidDetails { get; set; }
    }
}