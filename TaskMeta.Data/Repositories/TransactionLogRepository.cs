using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Models.Repositories
{
    public class TransactionLogRepository(ApplicationDbContext applicationDbContext, ICacheProvider cacheProvider, 
        ILogger<TransactionLog> logger)
        : RepositoryBase<TransactionLog>(applicationDbContext, cacheProvider, logger), ITransactionLogRepository
    {
        public IQueryable<TransactionLog> QueryTransactionsByUser(string id)
        {
            var query = Context.TransactionLogs.Where(x => x.TargetUserId == id)
                .Include(t => t.Category)
                .Include(t => t.SourceFund)
                .Include(t => t.TargetFund)
                .Include(t => t.TargetUser)
                .Include(t => t.CallingUser);
            return query!;
        }

        /// <summary>
        /// Adds a transaction to the log.
        /// </summary>
        /// <param name="transaction">The transaction to be logged.</param>
        /// <param name="commit">Indicates whether to commit the transaction immediately.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public void LogTransaction(Transaction transaction)
        {
            var transactionLog = new TransactionLog
            {
                Amount = transaction.Amount,
                CategoryId = transaction.CategoryId,
                Description = transaction.Description,
                SourceFundId = transaction.SourceFund?.Id,
                TargetFundId = transaction.TargetFund?.Id,
                TargetUserId = transaction.TargetUserId,
                CallingUserId = transaction.CallingUserId,
                PreviousAmount = transaction.PreviousAmount
            };

            AddJob(transactionLog);
        }
        public void Withdraw(Transaction transaction)
        {
            var sourceFund = transaction.SourceFund!;
            if (sourceFund.Balance < transaction.Amount)
            {
                throw new InvalidOperationException("Insufficient funds.");
            }
            transaction.PreviousAmount = sourceFund.Balance;
            sourceFund.Balance -= transaction.Amount;
            LogTransaction(transaction);
        }

        public void Deposit(Transaction transaction)
        {
            var targetFund = transaction.TargetFund!;
            transaction.PreviousAmount = targetFund.Balance;
            targetFund.Balance += transaction.Amount;
            LogTransaction(transaction);
        }
    }
}
