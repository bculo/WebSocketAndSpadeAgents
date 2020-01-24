using System.Threading.Tasks;
using VAS_API.Models;

namespace VAS_API.Interfaces.Services
{
    public interface IUserService
    {
        Task StartBuyer(User user);
    }
}
