using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Interfaces
{
    public interface ITransactionLogService : IEntityService<TransactionLog>
    {
        IQueryable<TransactionLog> QueryTransactionsByUser(string id);
        Task LogTransaction(Transaction transaction, bool commit = true);
    }
}
