using System;
using System.Collections.Generic;

namespace VAS_API.Models
{
    public sealed class VehicleAuction
    {
        public Guid AuctionId { get; set; }
        public DateTime? AuctionStart { get; set; }
        public DateTime? AuctionEnd { get; set; }
        public string Model { get; set; }
        public int HorsePower { get; set; }
        public int? BidIncrement { get; set; }
        public int? EndPrice { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public int Year { get; set; }
        public int Mileage { get; set; }
        public int StartPrice { get; set; }
        public VehicleType Type { get; set; }
        public string Color { get; set; }
        public VehicleCondition Condition { get; set; }
        public List<string> Buyers { get; set; } = new List<string>();
        public string Winner { get; set; }
    }
}
