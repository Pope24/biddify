using DataAccess;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Impl
{
    public class WinningService : IWinningService
    {
        private readonly IWinningRepository winningRepository;

        public WinningService(IWinningRepository winningRepository)
        {
            this.winningRepository = winningRepository;
        }

        public async Task<WinningEntity> GetWinnerByAuctionIdAsync(string auctionId)
        {
            return await winningRepository.GetWinnerByAuctionIdAsync(auctionId);
        }

        public async Task UpdateWinnerAsync(WinningEntity entity)
        {
            await winningRepository.UpdateWinnerAsync(entity);
        }
    }
}
