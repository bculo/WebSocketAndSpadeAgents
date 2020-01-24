using System;

namespace VAS_API.Contracts
{
    public class UpdateStartEndTimeRequest
    {
        public Guid AuctionId { get; set; }
        public string AuctionStart { get; set; }
        public string AuctionEnd { get; set; }
    }
}
