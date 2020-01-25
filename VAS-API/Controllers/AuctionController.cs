using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VAS_API.Contracts;
using VAS_API.Interfaces.Services;
using VAS_API.Models;

namespace VAS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionService _service;
        private readonly IUserService _userService;

        public AuctionController(IAuctionService service, IUserService userService)
        {
            _service = service;
            _userService = userService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetAuctionItems()
        {
            return Ok(await _service.GetVehicles());
        }

        [HttpPut("buyer")]
        public async Task<IActionResult> AddBuyer([FromBody] AddBuyerRequestModel auction)
        {
            return Ok(await _service.AddBuyer(auction.AuctionId ,auction.Buyer));
        }

        [HttpPut("newprice")]
        public async Task<IActionResult> ChangeItemPrice([FromBody] ChangePriceRequestModel auction)
        {
            return Ok(await _service.ChangePrice(auction.AuctionId, auction.EndPrice));
        }

        [HttpPut("finish")]
        public async Task<IActionResult> FinishAuctionForItem([FromBody] EndAuctionRequestModel auction)
        {
            return Ok(await _service.FinishAuction(auction.AuctionId, auction.EndPrice, auction.Winner));
        }

        [HttpPost("setdates")]
        public async Task<IActionResult> SetAuctionStartTime([FromBody] UpdateStartEndTimeRequest auction)
        {
            return Ok(await _service.SetAuctionStartEndAnd(auction.AuctionId, auction.AuctionStart, auction.AuctionEnd));
        }

        [HttpPost("startbuyer")]
        public async Task<IActionResult> StartBuyer([FromBody] StartBuyerRequestModel newBuyer)
        {
            await _userService.StartBuyer(newBuyer.Email);
            return Ok();
        }

        [HttpGet("getuserlist/{email}")]
        public async Task<IActionResult> StartBuyer([FromRoute] string email)
        {
            return Ok(await _userService.GetUserItems(email));
        }

        [HttpGet("getauctioneerinfo")]
        public async Task<IActionResult> AuctioneerInfo()
        {
            return Ok(await _userService.GetAuctioneerInfo());
        }

        [HttpGet("getitem")]
        public async Task<IActionResult> GetSingleItem([FromRoute] Guid id)
        {
            return Ok(await _service.GetVehcile(id));
        }
    }
}