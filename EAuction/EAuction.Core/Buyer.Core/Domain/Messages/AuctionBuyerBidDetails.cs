namespace EAuction.Buyer.Core.Domain.Messages
{
    public class AuctionBuyerBidDetails
    {
        public string ProductId { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public decimal BidAmount { get; set; }
        public string Email { get; set; }
    }
}