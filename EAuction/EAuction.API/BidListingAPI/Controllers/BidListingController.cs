using AutoMapper;
using EAuction.BidListing.API.Models;
using EAuction.BidListing.Core.Services;
using EAuction.Infrastructure.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EAuction.API.BidListing.Controllers
{
    [ApiController]
    [EAuctionAuthorize]
    [Route("api/v1")]
    public class BidListingController : ControllerBase
    {
        private readonly IBidListingService bidListingService;
        private readonly IMapper mapper;

        public BidListingController(IBidListingService bidListingService, IMapper mapper)
        {
            this.bidListingService = bidListingService;
            this.mapper = mapper;
        }

        [HttpGet("seller/show-bids/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductBidDetailsModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string productId)
        {
            var result = await this.bidListingService.GetProductAndBidDetails(productId);
            if (result == null) return NotFound();
            return Ok(this.mapper.Map<ProductBidDetailsModel>(result));
        }

        [HttpGet("IsAlive")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public bool IsAlive()
        {
            return true;
        }
    }
}