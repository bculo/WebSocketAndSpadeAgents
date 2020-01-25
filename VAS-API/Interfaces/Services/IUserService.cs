using System.Collections.Generic;
using System.Threading.Tasks;
using VAS_API.Models;

namespace VAS_API.Interfaces.Services
{
    public interface IUserService
    {
        Task StartBuyer(string buyerEmail);
        Task<List<WantedItem>> GetUserItems(string email);

        Task<string> GetAuctioneerInfo();
    }
}
