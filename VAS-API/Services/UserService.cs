using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        private readonly AvailabeBuyers _users;
        private readonly AuctionAuctioneerOptions _organizer;

        public List<User> ActiveUsers { get; set; }

        public UserService(IOptions<AuctionBuyerOptions> options,
            IOptions<AvailabeBuyers> buyers,
            IOptions<AuctionAuctioneerOptions> organizer)
        {
            ActiveUsers = new List<User>();
            _options = options.Value;
            _users = buyers.Value;
            _organizer = organizer.Value;
        }

        public async Task StartBuyer(string buyerEmail)
        {
            User user = _users.Buyers.FirstOrDefault(i => i.Email == buyerEmail);
            if (user == null)
                return;
            else
                AddNewActiveUser(user);

            await Task.Run(() =>
            {
                DirectoryInfo info = SolutionProvider.GetSolutionDirectoryPath();
                string projectName = info.Name;
                var executableFilePath = Path.Combine(info.FullName, projectName, _options.GetFilePath);

                try
                {
                    string jsonFormat = JsonConvert.SerializeObject(user.WantedItems);
                    string cmdArguments = $"/C \"{executableFilePath} {_options.PyUser} {user.Email} {_options.PyPassword} {user.Password}\"";

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

        public Task<List<WantedItem>> GetUserItems(string email)
        {
            User user = ActiveUsers.FirstOrDefault(i => i.Email == email);
            if (user == null)
                return Task.FromResult(new List<WantedItem>());

            return Task.FromResult(user.WantedItems);
        }

        public void AddNewActiveUser(User user)
        {
            ActiveUsers.RemoveAll(i => i.Email == user.Email);
            ActiveUsers.Add(user);
        }

        public Task<string> GetAuctioneerInfo()
        {
            return Task.FromResult(_organizer.Username);
        }
    }
}
