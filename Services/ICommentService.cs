using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentEntity>> GetCommentLazyLoadingAsync(string auctionId, int page = 1, int pageSize = 10);
        Task AddCommentAsync(CommentEntity entity);
    }
}
