using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using VAS_API.Extensions;
using VAS_API.Interfaces.Services;
using VAS_API.Models;
using VAS_API.Options;

namespace VAS_API.Services
{
    public class UserService : IUserService
    {
        private readonly AuctionBuyerOptions _options;

        public UserService(IOptions<AuctionBuyerOptions> options)
        {
            _options = options.Value;
        }

        public async Task StartBuyer(User user)
        {
            await Task.Run(() =>
            {
                DirectoryInfo info = SolutionProvider.GetSolutionDirectoryPath();
                string projectName = info.Name;
                var executableFilePath = Path.Combine(info.FullName, projectName, _options.GetFilePath);

                try
                {
                    string jsonFormat = JsonConvert.SerializeObject(user.WantedItems);
                    string cmdArguments = $"/C \"{executableFilePath} {_options.PyUser} {user.Email} {_options.PyPassword} {user.Password} {_options.PyBudget} {user.TotalBudget} {_options.PyItems} {jsonFormat}\"";

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
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }
    }
}
