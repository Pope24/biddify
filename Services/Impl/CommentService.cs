using DataAccess;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Impl
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task AddCommentAsync(CommentEntity entity)
        {
            await _commentRepository.AddCommentAsync(entity);
        }

        public async Task<IEnumerable<CommentEntity>> GetCommentLazyLoadingAsync(string auctionId, int page = 1, int pageSize = 10)
        {
            var comments = (await _commentRepository.GetCommentsByAuctionIdAsync(auctionId))
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
            return comments;
        }
    }
}
