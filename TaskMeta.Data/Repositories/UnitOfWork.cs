using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Logging;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Shared.Models.Repositories;

public class UnitOfWork(ApplicationDbContext context, ITaskDefinitionRepository taskDefinitionRepository,
    IJobRepository jobRepository, ITaskActivityRepository taskActivityRepository,
    ITaskWeekRepository taskWeekRepository, IFundRepository fundRepository,
    ITransactionLogRepository transactionLogRepository, IUserRepository userRepository, ILogger<IUnitOfWork> logger)
        : IUnitOfWork
{
    private ApplicationDbContext Context { get; set; } = context;
    private ITaskDefinitionRepository TaskDefinitionRepository { get;  set; } = taskDefinitionRepository;
    private IJobRepository JobRepository { get;  set; } = jobRepository;
    private ITaskActivityRepository TaskActivityRepository { get;  set; } = taskActivityRepository;
    private ITaskWeekRepository TaskWeekRepository { get;  set; } = taskWeekRepository;
    private IFundRepository FundRepository { get;  set; } = fundRepository;
    private ITransactionLogRepository TransactionLogRepository { get;  set; } = transactionLogRepository;
    private IUserRepository UserRepository { get;  set; } = userRepository;
    private ILogger Logger { get;  set; } = logger;

    public void SaveChanges()
    {
        Context.SaveChanges();
    }

    public TaskWeek GetOrCreateCurrentWeek(ApplicationUser user)
    {
        Guard.IsNotNull(user);

        var currentWeekStart = Tools.StartOfWeek(DateTime.Now);

        var currentWeek = TaskWeekRepository.Get(user.Id, currentWeekStart);
        if (currentWeek == null)
        {
            currentWeek = new TaskWeek()
            {
                WeekStartDate = Tools.StartOfWeek(DateTime.Now),
                UserId = user.Id,
                StatusId = 1,
                Value = 0
            };
            TaskWeekRepository.Add(currentWeek);
            SaveChanges();
            TaskActivityRepository.CreateForWeek(currentWeek, TaskDefinitionRepository.GetList(user));
            SaveChanges();

        }
        currentWeek.User = user;
        

        return currentWeek;
    }
    // Performs the necessary operations to set finalize the status of the task week
    /// </summary>
    /// <param name="taskWeek">The TaskWeek object to accept.</param>
    /// <param name="commit">A flag indicating whether to commit the changes to the database. Default is true.</param>
    public void AcceptWeek(TaskWeek taskWeek)
    {
        // Get the list of funds associated with the user
        List<Fund> fundList = FundRepository.GetFundsByUser(taskWeek.UserId);

        // Iterate through each fund and perform the necessary operations
        foreach (var fund in fundList)
        {
            // Calculate the deposit amount based on the task week value and fund allocation
            var depositAmount = taskWeek.Value * fund.Allocation.GetValueOrDefault() / 100;




            // Create a transaction log for the deposit
            TransactionLog transactionLog = new()
            {
                SourceFundId = null,
                TargetFundId = fund.Id,
                Amount = depositAmount,
                PreviousAmount = fund.Balance,
                CategoryId = Constants.Category.Deposit,
                CallingUserId = taskWeek.UserId,
                TargetUserId = taskWeek.UserId,
                Date = DateTime.Now,
                Description = "Weekly deposit from accepted tasks."
            };
            TransactionLogRepository.Add(transactionLog);

            // Update the fund balance by adding the deposit amount
            fund.Balance += depositAmount;
            FundRepository.Update(fund);
        }

        // Update the status of the task week to "Accepted"
        taskWeek.StatusId = 2;
        TaskWeekRepository.Update(taskWeek);
        SaveChanges();

    }
    /// <summary>
    /// Processes a transaction by updating the balances of the associated funds.
    /// </summary>
    /// <param name="transaction">The transaction to be processed.</param>
    public void ProcessTransaction(Transaction transaction)
    {
        switch (transaction.CategoryId)
        {
            case Constants.Category.Deposit:
                {
                    TransactionLogRepository.Deposit(transaction);
                    break;
                }
            case Constants.Category.Withdrawal:
                {
                    TransactionLogRepository.Withdraw(transaction);
                    break;
                }
            case Constants.Category.Transfer:
                {
                    TransactionLogRepository.Deposit(transaction);
                    TransactionLogRepository.Withdraw(transaction);
                    break;
                }
        }
        SaveChanges();
    }

    public void AddTaskDefinition(TaskDefinition taskDefinition)
    {
        TaskDefinitionRepository.Add(taskDefinition);
        SaveChanges();
    }

    public void UpdateJob(Job job, TaskWeek taskWeek)
    {
        JobRepository.Update(job);
        TaskWeekRepository.UpdateValue(taskWeek!, job.Value, job.Complete);
        SaveChanges();
    }

    public void UpdateTaskDefinition(TaskDefinition task)
    {
        TaskDefinitionRepository.Update(task);
        SaveChanges();
    }

    public void AddFund(Fund fund)
    {
        FundRepository.Add(fund);
        SaveChanges();
    }

    public void AddJob(Job job)
    {
        JobRepository.Add(job);
        SaveChanges();
    }

    public void DeleteFund(Fund fund)
    {
        FundRepository.Delete(fund);
        SaveChanges();
    }

    public void DeleteJob(Job job)
    {
        JobRepository.Delete(job);
        SaveChanges();
    }



    public void UpdateFund(Fund editFund)
    {
        FundRepository.Update(editFund);
        SaveChanges();
    }

    public void UpdateJob(Job job)
    {
        JobRepository.Update(job);
        SaveChanges();
    }

    public void UpdateTaskActivity(TaskActivity taskActivity)
    {
        TaskActivityRepository.Update(taskActivity);
        SaveChanges();
    }

    public void UpdateTaskWeekValue(TaskWeek taskWeek, decimal value, bool complete)
    {
        TaskWeekRepository.UpdateValue(taskWeek, value, complete);
        SaveChanges();
    }
    public void AddUser(ApplicationUser user)
    {
        Guard.IsNotNull(user);
        Guard.IsNotNull(user.NewPassword);
        UserRepository.Add(user);
    }   
    public void DeleteUser(ApplicationUser user)
    {
        Guard.IsNotNull(user);
        UserRepository.Delete(user);
    }
    public string UpdateUser(ApplicationUser user)
    {
        Guard.IsNotNull(user);
        UserRepository.Update(user);
        if(!String.IsNullOrEmpty(user.NewPassword)) return UserRepository.ResetPassword(user);
        return string.Empty;

    }
    public String ResetPassword(ApplicationUser user)
    {
        Guard.IsNotNull(user);
        Guard.IsNotNull(user.NewPassword);
        return UserRepository.ResetPassword(user);
    }

    public List<ApplicationUser>? GetContributors()
    {
        return UserRepository.GetContributors();
    }

    public List<Job>? GetCurrentJobs()
    {
        return JobRepository.GetCurrentJobs();
    }

    public List<TaskDefinition>? GetTaskDefinitionList()
    {
        return TaskDefinitionRepository.GetList();
    }

    public async Task<ApplicationUser?> GetCurrentUser()
    {
        return await UserRepository.GetCurrentUser();
    }

    public async Task<bool> IsAdmin()
    {
        return await UserRepository.IsAdmin();
    }

    public List<Job>? GetCurrentJobs(ApplicationUser user)
    {
        return JobRepository.GetCurrentJobs(user);
    }

    public List<ApplicationUser>? GetAllUsers()
    {
        return UserRepository.GetAllUsers();
    }

    public List<TransactionLog>? GetTransactionsByUser(string id, int page, int pageSize)
    {
        return TransactionLogRepository.GetTransactionsByUser(id, page, pageSize);
    }

    public (TaskWeek PreviousWeek, TaskWeek NextWeek) GetAdjacentTaskWeeks(TaskWeek taskWeek)
    {
        return TaskWeekRepository.GetAdjacent(taskWeek)!;
    }

    public List<TaskDefinition>? GetTaskDefinitionListByUser(ApplicationUser user)
    {
        return TaskDefinitionRepository.GetList(user);
    }

    public List<TaskActivity>? GetTaskDefinitionListByTaskWeek(TaskWeek taskWeek)
    {
        return TaskActivityRepository.GetListByTaskWeek(taskWeek);
    }

    public List<Fund>? GetFundsByUser(string id)
    {
        return FundRepository.GetFundsByUser(id);
    }

    public List<TaskActivity>? GetTaskActivityListByDate(DateOnly dateOnly, TaskWeek taskWeek)
    {
        return TaskActivityRepository.GetListByDate(dateOnly, taskWeek);
    }
}
