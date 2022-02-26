using AutoMapper;
using EAuction.Buyer.API.Models;
using EAuction.Buyer.Core.Domain;

namespace EAuction.Buyer.API
{
    public class AutoMapperBuyerProfile : Profile
    {
        public AutoMapperBuyerProfile()
        {
            CreateMap<AuctionBuyerBidModel, AuctionBuyer>();
            CreateMap<AuctionBuyerBidModel, AuctionBid>();
        }
    }
}