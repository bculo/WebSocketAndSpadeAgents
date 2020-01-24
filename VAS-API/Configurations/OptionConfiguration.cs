using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VAS_API.Interfaces.Installation;
using VAS_API.Options;

namespace VAS_API.Configurations
{
    public class OptionConfiguration : IInstaller
    {
        public void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuctionBuyerOptions>(configuration.GetSection(nameof(AuctionBuyerOptions)));
            services.Configure<AuctionOrganizerOptions>(configuration.GetSection(nameof(AuctionOrganizerOptions)));
        }
    }
}
