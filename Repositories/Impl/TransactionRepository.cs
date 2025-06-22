using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Impl
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BiddifyDbContext _dbContext;

        public TransactionRepository(BiddifyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddTransactionAsync(TransactionEntity entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsExistTransactionAsync(long orderCode)
        {
            return await _dbContext.Transactions.AnyAsync(t => t.TransactionCode == orderCode.ToString());
        }
    }
}
