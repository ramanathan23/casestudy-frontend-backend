using AutoMapper;
using EAuction.Seller.API.Models;
using EAuction.Seller.Core.Domain;

namespace EAuction.Seller.API
{
    public class AutoMapperSellerProfile : Profile
    {
        public AutoMapperSellerProfile()
        {
            CreateMap<SellerModel, AuctionProductSeller>();
            CreateMap<ProductModel, AuctionProduct>();
        }
    }
}