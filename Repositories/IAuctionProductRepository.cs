using DataAccess;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IAuctionProductRepository
    {
        Task<IEnumerable<AuctionProductEntity>> GetAuctionProductsAsync(
            string search = "", 
            EAuctionStatus? status = null,
            ECategoryProduct? category = null,
            bool? isCurrent = null);
            
        Task<AuctionProductEntity> GetAuctionProductByIdAsync(string id);
        Task<bool> AddAuctionProductAsync(AuctionProductEntity entity);
        Task<bool> UpdateAuctionProductAsync(AuctionProductEntity entity);
        Task<bool> DeleteAuctionProductAsync(string id);
    }
}
