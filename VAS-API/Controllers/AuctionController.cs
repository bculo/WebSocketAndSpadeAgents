using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VAS_API.Contracts;
using VAS_API.Interfaces.Services;
using VAS_API.Models;
using VAS_API.SignalR;

namespace VAS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionService _service;
        private readonly IUserService _userService;
        private readonly IHubContext<CommunicationHub> _hubContext;

        public AuctionController(IAuctionService service, IUserService userService, IHubContext<CommunicationHub> hubContext)
        {
            _service = service;
            _userService = userService;
            _hubContext = hubContext;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetAuctionItems()
        {
            return Ok(await _service.GetVehicles());
        }

        [HttpGet("updateclient")]
        public async Task<IActionResult> UpdateClient()
        {
            List<VehicleAuction> auctionItems = await _service.GetVehicles();
            await _hubContext.Clients.All.SendAsync("UpdateTimes", JsonConvert.SerializeObject(auctionItems));
            return Ok();
        }

        [HttpPut("buyer")]
        public async Task<IActionResult> AddBuyer([FromBody] AddBuyerRequestModel auction)
        {
            bool success = await _service.AddBuyer(auction.AuctionId, auction.Buyer);

            if (success)
            {
                await _hubContext.Clients.All.SendAsync("AddBuyer", JsonConvert.SerializeObject(auction));
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut("newprice")]
        public async Task<IActionResult> ChangeItemPrice([FromBody] ChangePriceRequestModel auction)
        {
            bool success = await _service.ChangePrice(auction.AuctionId, auction.EndPrice, auction.Winner);

            if (success)
            {
                await _hubContext.Clients.All.SendAsync("PriceChanged", JsonConvert.SerializeObject(auction));
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut("finish")]
        public async Task<IActionResult> FinishAuctionForItem([FromBody] ChangePriceRequestModel auction)
        {
            bool success = await _service.FinishAuction(auction.AuctionId, auction.EndPrice, auction.Winner);

            if (success)
            {
                await _hubContext.Clients.All.SendAsync("AuctionFinished", JsonConvert.SerializeObject(auction));
                return Ok();
            }

            return BadRequest();
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