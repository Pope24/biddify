using DataAccess;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IAuctionProductService
    {
        Task<(IEnumerable<AuctionProductEntity> Items, int TotalCount)> GetAuctionProductsAsync(
            string search = "", 
            EAuctionStatus? status = null,
            ECategoryProduct? category = null,
            bool? isCurrent = null,
            int pageNumber = 1,
            int pageSize = 9);
        Task<AuctionProductEntity> GetAuctionProductByIdAsync(string id);
        Task<bool> AddAuctionProductAsync(AuctionProductEntity entity);
        Task<bool> UpdateAuctionProductAsync(AuctionProductEntity entity);
        Task<bool> DeleteAuctionProductAsync(string id);
        Task<IEnumerable<AuctionProductEntity>> GetExpiredUnprocessedAuctionsAsync();
    }
}
