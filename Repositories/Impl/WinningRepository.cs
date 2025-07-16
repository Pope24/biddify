using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Impl
{
    public class WinningRepository : IWinningRepository
    {
        private readonly BiddifyDbContext _dbContext;

        public WinningRepository(BiddifyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WinningEntity> GetWinnerByAuctionIdAsync(string auctionId)
        {
            return await _dbContext.Winnings.FirstOrDefaultAsync(w => w.AuctionProductId == auctionId);
        }

        public async Task UpdateWinnerAsync(WinningEntity entity)
        {
            _dbContext.Winnings.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
