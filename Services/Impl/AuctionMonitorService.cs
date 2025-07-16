using DataAccess;
using Domain.Enums;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Impl
{
    public class AuctionMonitorService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<AuctionProductHub> _hubContext;

        public AuctionMonitorService(IServiceProvider serviceProvider, IHubContext<AuctionProductHub> hubContext)
        {
            _serviceProvider = serviceProvider;
            _hubContext = hubContext;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var auctionService = scope.ServiceProvider.GetRequiredService<IAuctionProductService>();
                var bidService = scope.ServiceProvider.GetRequiredService<IBidService>();
                var db = scope.ServiceProvider.GetRequiredService<BiddifyDbContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                var expiredAuctions = await auctionService.GetExpiredUnprocessedAuctionsAsync();

                foreach (var auction in expiredAuctions)
                {
                    var winningBid = auction.Bids.OrderByDescending(b => b.Amount).FirstOrDefault();
                    if (winningBid == null) continue;

                    var winner = winningBid.Bidder;

                    var win = new WinningEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        AuctionProductId = auction.Id,
                        WinnerId = winner.Id,
                        WinningBid = winningBid.Amount,
                        WonAt = DateTime.UtcNow
                    };

                    db.Winnings.Add(win);
                    auction.Status = EAuctionStatus.Ended;

                    await db.SaveChangesAsync();

                    await emailService.SendEmailAsync(
                        winner.Email,
                        "Chúc mừng bạn đã thắng phiên đấu giá!",
                        $"<h2>Xin chúc mừng {winner.DisplayName}!</h2><p>Bạn đã đấu giá thành công sản phẩm <strong>\"{auction.Title}\"</strong> với giá <strong>{winningBid.Amount:C}</strong>.</p><p>Vui lòng thanh toán để hoàn tất giao dịch.</p>" +
                        $"<a href=\"https://your-domain.com/pay/@auction.Id\"\r\n   style=\"padding:10px 20px; background-color:#4CAF50; color:white; text-decoration:none; border-radius:4px;\">\r\n   Thanh toán ngay\r\n</a>\r\n",
                        $"Xin chúc mừng {winner.DisplayName}! Bạn đã thắng đấu giá sản phẩm \"{auction.Title}\" với giá {winningBid.Amount:C}. Hãy thanh toán sớm."
                    );

                    var hub = scope.ServiceProvider.GetRequiredService<IHubContext<AuctionProductHub>>();
                    await hub.Clients.Group(auction.Id).SendAsync("AuctionEnded", winner.DisplayName, auction.Title);
                }

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}
