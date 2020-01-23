using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using VAS_API.Extensions;
using VAS_API.Interfaces.Installation;
using VAS_API.Options;

namespace VAS_API.Configurations
{
    public class AuctionConfiguration : IInstaller
    {
        public void Configure(IServiceCollection services, IConfiguration configuration)
        {
            var p = configuration.GetSectionApp<AuctionOrganizerOptions>();
            DirectoryInfo info = SolutionProvider.GetSolutionDirectoryPath();
            string projectName = info.Name;
            var executableFilePath = Path.Combine(info.FullName, projectName, p.GetFilePath);

            try
            {
                string cmdArguments = $"/C \"{executableFilePath} {p.PyUser} {p.Username} {p.PyPassword} {p.Password}\"";

                ProcessStartInfo myProcessStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    UseShellExecute = true,
                    Arguments = cmdArguments
                };

                using var myProcess = new Process
                {
                    StartInfo = myProcessStartInfo
                };

                myProcess.Start();
            }
            catch (Exception e)
            {
                throw new Exception("Problem kod pokretanja aukcije");
            }
        }
    }
}
