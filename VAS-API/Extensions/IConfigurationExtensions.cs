using Microsoft.Extensions.Configuration;

namespace VAS_API.Extensions
{
    public static class IConfigurationExtensions
    {
        public static T GetSectionApp<T>(this IConfiguration configuration)
        {
            string sectionName = typeof(T).Name;
            T section = configuration.GetSection(sectionName).Get<T>();
            return section;
        } 
    }
}
