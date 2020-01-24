using System;

namespace VAS_API.Contracts
{
    public class ChangePriceRequestModel
    {
        public Guid AuctionId { get; set; }
        public int EndPrice { get; set; }
    }
}
