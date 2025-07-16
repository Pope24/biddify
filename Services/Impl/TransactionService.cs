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
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            this.transactionRepository = transactionRepository;
        }

        public async Task AddTransactionAsync(string id, long orderCode, string userId, ETransactionType type, ETransactionStatus status, decimal amount, DateTime createdAt)
        {
            var transaction = new TransactionEntity
            {
                Id = id,
                TransactionCode = orderCode.ToString(),
                UserId = userId,
                Type = type,
                Amount = amount,
                CreatedAt = createdAt,
                Status = status,
            };
            await transactionRepository.AddTransactionAsync(transaction);
        }

        public async Task<TransactionEntity> GetTransactionByCodeAsync(long code)
        {
            return await transactionRepository.GetTransactionByCodeAsync(code);
        }

        public async Task<IEnumerable<TransactionEntity>> GetTransactionsByUserIdAsync(string userId, string? search = "")
        {
            return await transactionRepository.GetTransactionsByUserIdAsync(userId, search);
        }

        public async Task<bool> IsExistTransactionAsync(long orderCode)
        {
            return await transactionRepository.IsExistTransactionAsync(orderCode);
        }
        public async Task UpdateTransactionAsync(TransactionEntity entity)
        {
            await transactionRepository.UpdateTransactionAsync(entity);
        }
    }
}
