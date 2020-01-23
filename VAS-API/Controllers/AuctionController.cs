using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VAS_API.Interfaces.Services;
using VAS_API.Models;

namespace VAS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionService _service;

        public AuctionController(IAuctionService service)
        {
            _service = service;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetAuctionItems()
        {
            return Ok(await _service.GetVehicles());
        }

        [HttpPut("buyer")]
        public async Task<IActionResult> AddBuyer(VehicleAuction auction)
        {
            return Ok(await _service.AddBuyer(auction));
        }

        [HttpPut("finish")]
        public async Task<IActionResult> FinishAuctionForItem(VehicleAuction auction)
        {
            return Ok(await _service.FinishAuction(auction));
        }

        [HttpPut("setdates")]
        public async Task<IActionResult> SetAuctionStartTime(VehicleAuction auction)
        {
            return Ok(await _service.SetAuctionStartEndAnd(auction));
        }

        [HttpGet("getitem")]
        public async Task<IActionResult> GetSingleItem([FromRoute] Guid id)
        {
            return Ok(await _service.GetVehcile(id));
        }
    }
}