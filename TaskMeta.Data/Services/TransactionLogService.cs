﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;

namespace TaskMeta.Data.Services
{
    public class TransactionLogService : EntityService<TransactionLog>, ITransactionLogService
    {
        public TransactionLogService(ApplicationDbContext applicationDbContext, IUserService userService, 
            ILogger<EntityService<TransactionLog>> logger) 
            : base(applicationDbContext, userService, logger)
        {
        }

        public IQueryable<TransactionLog> QueryTransactionsByUser(string id)
        {
            var query = Context.Set<TransactionLog>().Where(x => x.TargetUserId == id)
                .Include(t=>t.Category)
                .Include(t=>t.SourceFund)
                .Include(t=>t.TargetFund)
                .Include(t=>t.TargetUser)
                .Include(t=>t.CallingUser);
            return query!;
        }

        public async Task LogTransaction(Transaction transaction, bool commit = true)
        {
            var transactionLog = new TransactionLog
            {
                Amount = transaction.Amount,
                CategoryId = transaction.CategoryId,
                Description = transaction.Description,
                SourceFundId = transaction.SourceFund?.Id,
                TargetFundId = transaction.TargetFund?.Id,
                TargetUserId = transaction.TargetUserId,
                CallingUserId = transaction.CallingUserId
            };

            await AddAsync(transactionLog, commit);
        }
    }
}