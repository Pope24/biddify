using DataAccess;
using Microsoft.AspNetCore.SignalR;

namespace Biddify.SignalR
{
    public class AuctionProductHub : Hub
    {
        public async Task SendNewAuctionProduct(AuctionProductEntity product)
        {
            await Clients.All.SendAsync("ReceiveProduct", product);
        }
    }
}
