using DataAccess;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ITransactionService
    {
        Task AddTransactionAsync(string id, long orderCode, string userId, ETransactionType type, ETransactionStatus status, decimal amount, DateTime createdAt);
        Task<bool> IsExistTransactionAsync(long orderCode);
        Task<TransactionEntity> GetTransactionByCodeAsync(long code);
        Task<IEnumerable<TransactionEntity>> GetTransactionsByUserIdAsync(string userId, string? search = "");
        Task UpdateTransactionAsync(TransactionEntity entity);
    }
}