using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IWinningService
    {
        Task<WinningEntity> GetWinnerByAuctionIdAsync(string auctionId);
        Task UpdateWinnerAsync(WinningEntity entity);
    }
}
