using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VAS_API.Interfaces.Installation;
using VAS_API.Interfaces.Services;
using VAS_API.Services;

namespace VAS_API.Configurations
{
    public class DIConfiguration : IInstaller
    {
        public void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAuctionService, AuctionService>();
            services.AddTransient<IUserService, UserService>();
        }
    }
}
