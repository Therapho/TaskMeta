using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Interfaces
{
    public interface ITransactionLogRepository : IRepositoryBase<TransactionLog>
    {
        List<TransactionLog> GetTransactionsByUser(string id, int page, int pageSize);
        void LogTransaction(Transaction transaction);
        void Deposit(Transaction transaction);
        void Withdraw(Transaction transaction);
    }
}
