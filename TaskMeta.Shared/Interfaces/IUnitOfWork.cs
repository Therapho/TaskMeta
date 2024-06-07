using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Interfaces;

public interface IUnitOfWork
{
    #region Repositories
    IFundRepository FundRepository { get; }
    IJobRepository JobRepository { get; }
    ITaskActivityRepository TaskActivityRepository { get; }
    ITaskDefinitionRepository TaskDefinitionRepository { get; }
    ITaskWeekRepository TaskWeekRepository { get; }
    ITransactionLogRepository TransactionLogRepository { get; }
    IUserRepository UserRepository { get; }
    #endregion

    Task SaveChanges();
    
    #region Compound Actions
    Task AcceptWeek(TaskWeek taskWeek);
    Task<TaskWeek> GetOrCreateCurrentWeek(ApplicationUser user);
    void Process(Transaction transaction);
    #endregion

}