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

    
    #region Compound Actions
    void AcceptWeek(TaskWeek taskWeek);
    void AddFund(Fund editFund);
    void AddJob(Job Job);
    void AddTaskDefinition(TaskDefinition taskDefinition);
    void DeleteFund(Fund fund);
    void DeleteJob(Job job);
    TaskWeek GetOrCreateCurrentWeek(ApplicationUser user);
    void ProcessTransaction(Transaction transaction);
    void UpdateFund(Fund fund);
    void UpdateJob(Job job);
    void UpdateTaskActivity(TaskActivity taskActivity);
    void UpdateTaskDefinition(TaskDefinition task);
    void UpdateTaskWeekValue(TaskWeek taskWeek, decimal value, bool complete);
    #endregion

}