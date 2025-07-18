using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IWinningRepository
    {
        Task<WinningEntity> GetWinnerByAuctionIdAsync(string auctionId);
        Task UpdateWinnerAsync(WinningEntity entity);
    }
}
