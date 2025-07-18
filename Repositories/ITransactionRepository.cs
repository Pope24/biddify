using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ITransactionRepository
    {
        Task AddTransactionAsync(TransactionEntity entity);
        Task<bool> IsExistTransactionAsync(long orderCode);
        Task<TransactionEntity> GetTransactionByCodeAsync(long code);
        Task UpdateTransactionAsync(TransactionEntity entity);
        Task<IEnumerable<TransactionEntity>> GetTransactionsByUserIdAsync(string userId, string? search = "");
    }
}
