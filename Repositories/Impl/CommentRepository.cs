using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Impl
{
    public class CommentRepository : ICommentRepository
    {
        public readonly BiddifyDbContext _dbContext;

        public CommentRepository(BiddifyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddCommentAsync(CommentEntity comment)
        {
            await _dbContext.AddAsync(comment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<CommentEntity>> GetCommentsAsync()
        {
            return await _dbContext.Comments.ToListAsync();
        }

        public async Task<IEnumerable<CommentEntity>> GetCommentsByAuctionIdAsync(string auctionId)
        {
            return await _dbContext.Comments.Where(c => c.AuctionProductId == auctionId).Include(c => c.User).ToListAsync();
        }
    }
}
