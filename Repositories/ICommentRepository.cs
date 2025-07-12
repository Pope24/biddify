using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ICommentRepository
    {
        Task<IEnumerable<CommentEntity>> GetCommentsAsync();
        Task<IEnumerable<CommentEntity>> GetCommentsByAuctionIdAsync(string auctionId);
        Task AddCommentAsync(CommentEntity comment);
    }
}
