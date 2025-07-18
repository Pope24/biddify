using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IBidService
    {
        Task<bool> AddBidAsync(BidEntity entity);
        Task<BidEntity> GetBidByIdAsync(string id);
        Task<IEnumerable<BidEntity>> GetBidsByUserIdAsync(string userId);
    }
}
