using AutoMapper;
using EAuction.BidListing.API.Models;
using EAuction.BidListing.Core.Domain;

namespace EAuction.BidListing.API
{
    public class AutoMapperBidListingProfile : Profile
    {
        public AutoMapperBidListingProfile()
        {
            CreateMap<ProductAndBidDetails, ProductBidDetailsModel>();
            CreateMap<BidDetails, BidDetailsModel>();
        }
    }
}