using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VAS_API.Interfaces.Installation
{
    public interface IInstaller
    {
        void Configure(IServiceCollection services, IConfiguration configuration);
    }
}
