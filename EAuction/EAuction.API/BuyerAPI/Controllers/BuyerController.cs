using AutoMapper;
using EAuction.Buyer.API.Models;
using EAuction.Buyer.Core.Domain;
using EAuction.Buyer.Core.Services;
using EAuction.Infrastructure.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EAuction.Buyer.API.Controllers
{
    [ApiController]
    [EAuctionAuthorize]
    [Route("api/v1/[controller]")]
    public class BuyerController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IBuyerService buyerService;

        public BuyerController(IBuyerService buyerService, IMapper mapper)
        {
            this.mapper = mapper;
            this.buyerService = buyerService;
        }

        [HttpPost("place-bid")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(AuctionBuyerBidModel bid)
        {
            if (await this.buyerService.AddBidAsync(mapper.Map<AuctionBuyer>(bid), mapper.Map<AuctionBid>(bid)))
            {
                return Ok(new ResponseModel() { Message = "Your Request has been recieved sucessfully. It will be processed shortly" });
            }
            return BadRequest();
        }

        [HttpPatch("update-bid/{productId}/{phone}/{newBidAmount}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string productId, string phone, decimal newBidAmount)
        {
            if (await this.buyerService.UpdateBidAsync(phone, productId, newBidAmount))
            {
                return Ok(new ResponseModel() { Message = "Your Request has been recieved sucessfully. It will be processed shortly" });
            }
            return BadRequest();
        }

        [HttpGet("IsAlive")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public bool IsAlive()
        {
            return true;
        }
    }
}