﻿using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Interfaces
{
    public interface ITransactionLogRepository : IRepositoryBase<TransactionLog>
    {
        IQueryable<TransactionLog> QueryTransactionsByUser(string id);
        void LogTransaction(Transaction transaction);
        void Deposit(Transaction transaction);
        void Withdraw(Transaction transaction);
    }
}
