using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using VAS_API.Interfaces.Services;

namespace VAS_API.SignalR
{
    public class CommunicationHub : Hub
    {
        private readonly IAuctionService _service;

        public CommunicationHub(IAuctionService service)
        {
            _service = service;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("UpdateTimes", JsonConvert.SerializeObject(_service.GetVehicles()));
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
