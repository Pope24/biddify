using DataAccess;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Impl
{
    public class AuctionProductRepository : IAuctionProductRepository
    {
        private readonly BiddifyDbContext dbContext;

        public AuctionProductRepository(BiddifyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> AddAuctionProductAsync(AuctionProductEntity entity)
        {
            await dbContext.AuctionProducts.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAuctionProductAsync(string id)
        {
            var auctionProduct = await dbContext.AuctionProducts.FindAsync(id);
            if (auctionProduct == null)
            {
                return false;
            }

            dbContext.AuctionProducts.Remove(auctionProduct);
            var result = await dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<AuctionProductEntity> GetAuctionProductByIdAsync(string id)
        {
            return await dbContext.AuctionProducts.Where(p => p.Id == id)
                .Include(p => p.Seller)
                .Include(p => p.Bids)
                .ThenInclude(b => b.Bidder)
                .FirstAsync();
        }

        public async Task<IEnumerable<AuctionProductEntity>> GetAuctionProductsAsync(
          string search = "",
          EAuctionStatus? status = null,
          ECategoryProduct? category = null,
          bool? isCurrent = null)
        {
            var query = dbContext.AuctionProducts.AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.Title.Contains(search.Trim()));
            }

            // Apply status filter
            if (status.HasValue)
            {
                query = query.Where(p => p.Status == status.Value);
            }

            // Apply category filter
            if (category.HasValue)
            {
                query = query.Where(p => p.CategoryProduct == category.Value);
            }

            // Apply time filter
            if (isCurrent.HasValue)
            {
                var currentTime = DateTime.UtcNow;
                if (isCurrent.Value)
                {
                    // Current auctions: start time <= now < end time
                    query = query.Where(p =>
                        p.StartTime <= currentTime &&
                        p.EndTime > currentTime);
                }
                else
                {
                    // Upcoming auctions: start time > now
                    query = query.Where(p => p.StartTime > currentTime);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<bool> UpdateAuctionProductAsync(AuctionProductEntity entity)
        {
            dbContext.AuctionProducts.Update(entity);
            var result = await dbContext.SaveChangesAsync();
            return result > 0;
        }
    }
}
