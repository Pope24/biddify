using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IAuctionProductRepository
    {
        Task<IEnumerable<AuctionProductEntity>> GetAuctionProductsAsync(string search = "");
        Task<AuctionProductEntity> GetAuctionProductByIdAsync(string id);
        Task<bool> AddAuctionProductAsync(AuctionProductEntity entity);
        Task<bool> UpdateAuctionProductAsync(AuctionProductEntity entity);
        Task<bool> DeleteAuctionProductAsync(string id);
    }
}
