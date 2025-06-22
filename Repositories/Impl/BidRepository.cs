using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Impl
{
    public class BidRepository : IBidRepository
    {
        private readonly BiddifyDbContext _dbContext;

        public BidRepository(BiddifyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddBidAsync(BidEntity entity)
        {
            await _dbContext.Bids.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<BidEntity> GetBidByIdAsync(string id)
        {
            return await _dbContext.Bids.Include(b => b.Bidder).FirstAsync(b => b.Id == id);
        }
    }
}
