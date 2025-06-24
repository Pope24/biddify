using Common;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Service;
namespace Biddify.SignalR
{
    public class CommentHub : Hub
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<UserEntity> _userManager;

        public CommentHub(ICommentService commentService, UserManager<UserEntity> userManager)
        {
            _commentService = commentService;
            _userManager = userManager;
        }

        public async Task SendComment(string auctionId, string userId, string message)
        {
            var user = await _userManager.FindByNameAsync(userId);
            if (user == null) return;

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
