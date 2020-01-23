using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VAS_API.Interfaces.Installation;

namespace VAS_API.Configurations
{
    public class MVCConfiguration : IInstaller
    {
        public void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors();
            services.AddControllers();
        }
    }
}
