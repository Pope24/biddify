using Microsoft.AspNetCore.SignalR;

namespace Service.SignalR
{
    public class BidHub : Hub
    {
        public async Task SendBidUpdate(string itemId)
        {
            await Clients.All.SendAsync("ReceiveBidAdd", itemId);
        }
    }
}
