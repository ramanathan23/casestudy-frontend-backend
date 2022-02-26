using AutoMapper;
using EAuction.Infrastructure.Common;
using EAuction.Seller.API.Models;
using EAuction.Seller.Core.Domain;
using EAuction.Seller.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EAuction.Seller.API.Controllers
{
    [ApiController]
    [EAuctionAuthorize]
    [Route("api/v1/[controller]")]
    public class SellerController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ISellerService sellerService;

        public SellerController(IMapper mapper, ISellerService sellerService)
        {
            this.mapper = mapper;
            this.sellerService = sellerService;
        }

        [HttpPost("add-product")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(AuctionProductModel product)
        {
            if (await this.sellerService.AddProductAsync(mapper.Map<AuctionProduct>(product.Product), mapper.Map<AuctionProductSeller>(product.Seller)))
            {
                return Ok(new ResponseModel() { Message = "Your request has been processed successfully" });
            }
            return BadRequest();
        }

        [HttpDelete("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string productId)
        {
            if (await this.sellerService.DeleteProductFromAuctionAsync(productId))
            {
                return Ok(new ResponseModel() { Message = "Your request has been receieved successfully. It will be processed shortly" });
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