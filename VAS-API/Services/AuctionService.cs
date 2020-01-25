using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VAS_API.Interfaces.Services;
using VAS_API.Models;

namespace VAS_API.Services
{
    public class AuctionService : IAuctionService
    {
        public List<VehicleAuction> AuctionItems { get; set; }

        public AuctionService()
        {
            AuctionItems = new List<VehicleAuction>();
            FillAuctionList();
        }

        public Task<VehicleAuction> GetVehcile(Guid id)
        {
            return Task.FromResult(AuctionItems.FirstOrDefault(i => i.AuctionId == id));
        }

        public Task<List<VehicleAuction>> GetVehicles()
        {
            return Task.FromResult(AuctionItems);
        }

        public Task<bool> AddBuyer(Guid id, string buyerIdentifier)
        {
            var result = AuctionItems.FirstOrDefault(i => i.AuctionId == id);

            if (result == null)
                return Task.FromResult(false);

            if (result.Buyers.All(i => i != buyerIdentifier))
                result.Buyers.Add(buyerIdentifier);

            return Task.FromResult(true);
        }

        public Task<bool> FinishAuction(Guid id, int endPrice, string winner)
        {
            var result = AuctionItems.FirstOrDefault(i => i.AuctionId == id);

            if (result == null)
                return Task.FromResult(false);

            result.EndPrice = endPrice;
            result.Winner = winner;

            return Task.FromResult(true);
        }

        public Task<bool> SetAuctionStartEndAnd(Guid id, string start, string end)
        {
            var result = AuctionItems.FirstOrDefault(i => i.AuctionId == id);

            if (result == null)
                return Task.FromResult(false);

            result.AuctionStart = DateTime.Parse(start);
            result.AuctionEnd = DateTime.Parse(end);

            return Task.FromResult(true);
        }

        public Task<bool> ChangePrice(Guid auctionId, int endPrice)
        {
            var result = AuctionItems.FirstOrDefault(i => i.AuctionId == auctionId);

            if (result == null)
                return Task.FromResult(false);

            result.EndPrice = endPrice;

            return Task.FromResult(true);
        }

        private void FillAuctionList()
        {
            //first car
            AuctionItems.Add(new VehicleAuction
            {
                AuctionId = Guid.NewGuid(),
                Color = "Red",
                HorsePower = 150,
                Condition = VehicleCondition.GOOD,
                Model = "BMW F80",
                Type = VehicleType.CAR,
                Year = 2015,
                StartPrice = 44000,
                Mileage = 30000,
                BidIncrement = 1000,
            });

            //second car
            AuctionItems.Add(new VehicleAuction
            {
                AuctionId = Guid.NewGuid(),
                Color = "Black",
                HorsePower = 230,
                Condition = VehicleCondition.GREAT,
                Model = "Mercedes C63",
                Type = VehicleType.CAR,
                Year = 2016,
                StartPrice = 48000,
                Mileage = 80000,
                BidIncrement = 700,
            });

            //third car
            AuctionItems.Add(new VehicleAuction
            {
                AuctionId = Guid.NewGuid(),
                Color = "Silver",
                HorsePower = 300,
                Condition = VehicleCondition.GOOD,
                Model = "BMW E36 M3",
                Type = VehicleType.CAR,
                Year = 1995,
                StartPrice = 20000,
                Mileage = 120000,
            });

            //fourth car
            AuctionItems.Add(new VehicleAuction
            {
                AuctionId = Guid.NewGuid(),
                Color = "Green",
                HorsePower = 136,
                Condition = VehicleCondition.GOOD,
                Model = "Kawasaki Ninja 636",
                Type = VehicleType.MOTORBIKE,
                Year = 2019,
                StartPrice = 57500,
                Mileage = 12000,
                BidIncrement = 500,
            });

            //fifth car
            AuctionItems.Add(new VehicleAuction
            {
                AuctionId = Guid.NewGuid(),
                Color = "White",
                HorsePower = 150,
                Condition = VehicleCondition.GOOD,
                Model = "PEUGEOT 308 2.0",
                Type = VehicleType.CAR,
                Year = 2016,
                StartPrice = 28000,
                Mileage = 120000,
                BidIncrement = 300,
            });
        }
    }
}
