using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VAS_API.Models;

namespace VAS_API.Interfaces.Services
{
    public interface IAuctionService
    {
        Task<List<VehicleAuction>> GetVehicles();
        Task<bool> SetAuctionStartEndAnd(VehicleAuction vehicle);
        Task<bool> AddBuyer(VehicleAuction vehicle);
        Task<bool> FinishAuction(VehicleAuction vehicle);
        Task<VehicleAuction> GetVehcile(Guid id);
    }
}
