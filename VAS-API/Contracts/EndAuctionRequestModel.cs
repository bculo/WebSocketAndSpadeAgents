using System;

namespace VAS_API.Contracts
{
    public class EndAuctionRequestModel
    {
        public Guid AuctionId { get; set; }
        public int EndPrice { get; set; }
        public string Winner { get; set; }
    }
}
