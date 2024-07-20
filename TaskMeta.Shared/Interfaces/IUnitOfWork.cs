using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Interfaces;

public interface IUnitOfWork
{
    #region Repositories
    //IFundRepository FundRepository { get; }
    //IJobRepository JobRepository { get; }
    //ITaskActivityRepository TaskActivityRepository { get; }
    //ITaskDefinitionRepository TaskDefinitionRepository { get; }
    //ITaskWeekRepository TaskWeekRepository { get; }
    //ITransactionLogRepository TransactionLogRepository { get; }
    //IUserRepository UserRepository { get; }
    #endregion

    
    #region Compound Actions
    void AcceptWeek(TaskWeek taskWeek);
    void AddFund(Fund editFund);
    void AddJob(Job Job);
    void AddTaskDefinition(TaskDefinition taskDefinition);
    void AddUser(ApplicationUser user);
    string UpdateUser(ApplicationUser user);
    void DeleteUser(ApplicationUser user);
    string ResetPassword(ApplicationUser user);
    void DeleteFund(Fund fund);
    void DeleteJob(Job job);
    TaskWeek GetOrCreateCurrentWeek(ApplicationUser user);
    void ProcessTransaction(Transaction transaction);
    void UpdateFund(Fund fund);
    void UpdateJob(Job job);
    void UpdateTaskActivity(TaskActivity taskActivity);
    void UpdateTaskDefinition(TaskDefinition task);
    void UpdateTaskWeekValue(TaskWeek taskWeek, decimal value, bool complete);
    List<Job>? GetCurrentJobs(ApplicationUser user);
    List<ApplicationUser>? GetContributors();
    List<TaskDefinition>? GetTaskDefinitionList();
    Task<ApplicationUser?> GetCurrentUser();
    Task<bool> IsAdmin();
    List<ApplicationUser>? GetAllUsers();
    List<TransactionLog>? GetTransactionsByUser(string id, int v1, int v2);
    (TaskWeek PreviousWeek, TaskWeek NextWeek) GetAdjacentTaskWeeks(TaskWeek taskWeek);
    List<TaskDefinition>? GetTaskDefinitionListByUser(ApplicationUser user);
    List<TaskActivity>? GetTaskDefinitionListByTaskWeek(TaskWeek taskWeek);
    List<Fund>? GetFundsByUser(string id);
    List<TaskActivity>? GetTaskActivityListByDate(DateOnly dateOnly, TaskWeek taskWeek);
    #endregion

}