using System;

namespace VAS_API.Contracts
{
    public class AddBuyerRequestModel
    {
        public Guid AuctionId { get; set; }
        public string Buyer { get; set; }
    }
}
