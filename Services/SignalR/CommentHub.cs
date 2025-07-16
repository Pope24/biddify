using Common;
using DataAccess;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Service;
namespace Service.SignalR
{
    public class CommentHub : Hub
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IAuctionProductService _auctionProductService;

        public CommentHub(ICommentService commentService, UserManager<UserEntity> userManager, IAuctionProductService auctionProductService)
        {
            _commentService = commentService;
            _userManager = userManager;
            _auctionProductService = auctionProductService;
        }

        public async Task SendComment(string auctionId, string userId, string message)
        {
            var user = await _userManager.FindByNameAsync(userId);
            if (user == null)
            {
                await Clients.Caller.SendAsync("CommentError", "User not found.");
                return;
            }
            var auction = await _auctionProductService.GetAuctionProductByIdAsync(auctionId);
            if (auction.Status != EAuctionStatus.Active)
            {
                await Clients.Caller.SendAsync("CommentError", $"Auction is in '{EnumHelper.ToDisplayString(auction.Status)}' status.");
                return;
            }
            var now = DateTime.UtcNow;
            if (now < auction.StartTime || now > auction.EndTime)
            {
                await Clients.Caller.SendAsync("CommentError", "Auction is not active at this time.");
                return;
            }

            var comment = new CommentEntity
            {
                Id = Guid.NewGuid().ToString(),
                AuctionProductId = auctionId,
                UserId = user.Id,
                Content = message,
                CreatedAt = DateTime.UtcNow
            };
            await _commentService.AddCommentAsync(comment);
            await Clients.Group(auctionId).SendAsync("ReceiveComment", user.DisplayName, comment.Content, comment.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"));
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var auctionId = httpContext?.Request.Query["auctionId"];

            if (!string.IsNullOrEmpty(auctionId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, auctionId);
            }

            await base.OnConnectedAsync();
        }
    }
}
