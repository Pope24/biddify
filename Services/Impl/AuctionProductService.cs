using Common.Exceptions;
using DataAccess;
using Domain.Enums;
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

        public async Task<AuctionProductEntity> GetAuctionProductByIdAsync(string id)
        {
            id.ThrowIfNull("Auction product ID cannot be null");
            
            var auction = await auctionProductRepository.GetAuctionProductByIdAsync(id);
            
            // Still throw NotFound exception for individual item lookups
            // This will trigger a proper 404 Not Found response
            auction.ThrowIfNull($"Auction product with ID {id} not found");
            
            return auction;
        }

        public async Task<(IEnumerable<AuctionProductEntity> Items, int TotalCount)> GetAuctionProductsAsync(
            string search = "", 
            EAuctionStatus? status = null,
            ECategoryProduct? category = null,
            bool? isCurrent = null,
            int pageNumber = 1,
            int pageSize = 9)
        {
            // Get all auctions with filters
            var allAuctions = await auctionProductRepository.GetAuctionProductsAsync(search, status, category, isCurrent);
            
            // Ensure we have a non-null collection
            var auctions = allAuctions ?? Enumerable.Empty<AuctionProductEntity>();
            
            // Calculate total count for pagination
            int totalCount = auctions.Count();
            
            // Apply pagination
            var paginatedAuctions = auctions
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
                
            return (paginatedAuctions, totalCount);
        }

        public Task<bool> UpdateAuctionProductAsync(AuctionProductEntity entity)
        {
            return auctionProductRepository.UpdateAuctionProductAsync(entity);
        }

        public Task<bool> DeleteAuctionProductAsync(string id)  
        {
            return auctionProductRepository.DeleteAuctionProductAsync(id);
        }
    }
}
