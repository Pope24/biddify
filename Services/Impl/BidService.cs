using DataAccess;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Impl
{
    public class BidService : IBidService
    {
        private readonly IBidRepository bidRepository;

        public BidService(IBidRepository _bidRepository)
        {
            this.bidRepository = _bidRepository;
        }

        public Task<bool> AddBidAsync(BidEntity entity)
        {
            return bidRepository.AddBidAsync(entity);
        }

        public async Task<BidEntity> GetBidByIdAsync(string id)
        {
            return await bidRepository.GetBidByIdAsync(id);
        }
    }
}
