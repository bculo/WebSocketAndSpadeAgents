using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VAS_API.Models;

namespace VAS_API.Interfaces.Services
{
    public interface IAuctionService
    {
        Task<List<VehicleAuction>> GetVehicles();
        Task<bool> SetAuctionStartEndAnd(Guid id, string start, string end);
        Task<bool> AddBuyer(Guid id, string buyerIdentifier);
        Task<bool> FinishAuction(Guid id, int endPrice, string winner);
        Task<VehicleAuction> GetVehcile(Guid id);
        Task<bool> ChangePrice(Guid auctionId, int endPrice, string winner);
    }
}
