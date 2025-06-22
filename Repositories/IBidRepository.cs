using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IBidRepository
    {
        Task<bool> AddBidAsync(BidEntity entity);
        Task<BidEntity> GetBidByIdAsync(string id);
    }
}
