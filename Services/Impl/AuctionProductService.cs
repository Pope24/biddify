using DataAccess;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Impl
{
    public class AuctionProductService : IAuctionProductService
    {
        private readonly IAuctionProductRepository auctionProductRepository;

        public AuctionProductService(IAuctionProductRepository auctionProductRepository)
        {
            this.auctionProductRepository = auctionProductRepository;
        }

        public Task<bool> AddAuctionProductAsync(AuctionProductEntity entity)
        {
            return auctionProductRepository.AddAuctionProductAsync(entity);
        }

        public Task<AuctionProductEntity> GetAuctionProductByIdAsync(string id)
        {
            return auctionProductRepository.GetAuctionProductByIdAsync(id);
        }

        public Task<IEnumerable<AuctionProductEntity>> GetAuctionProductsAsync(string search = "")
        {
            return auctionProductRepository.GetAuctionProductsAsync(search);
        }
    }
}
