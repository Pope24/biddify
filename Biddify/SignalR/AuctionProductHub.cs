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
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var auctionId = httpContext.Request.Query["auctionId"];
            if (!string.IsNullOrEmpty(auctionId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, auctionId);
            }
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = Context.GetHttpContext();
            var auctionId = httpContext.Request.Query["auctionId"];

            if (!string.IsNullOrEmpty(auctionId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, auctionId);
            }
            await base.OnDisconnectedAsync(exception);
        }
        public async Task NotifyWinner(string auctionId, string winnerName, string productName)
        {
            Console.WriteLine($"Notifying winner for AuctionId: {auctionId}, Winner: {winnerName}, Product: {productName}");
            await Clients.Group(auctionId).SendAsync("AuctionEnded", winnerName, productName);
        }
    }
}
