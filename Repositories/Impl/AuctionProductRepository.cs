using DataAccess;
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

        public async Task<AuctionProductEntity> GetAuctionProductByIdAsync(string id)
        {
            return await dbContext.AuctionProducts.Where(p => p.Id == id).Include(p => p.Seller).FirstAsync();
        }

        public async Task<IEnumerable<AuctionProductEntity>> GetAuctionProductsAsync(string search = "")
        {
            var res = await dbContext.AuctionProducts
                .Where(p => p.Title.Contains(search.Trim()))
                .ToListAsync();
            return res;
        }
    }
}
