using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Logging;
using TaskMeta.Shared;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Shared.Models.Repositories;

public class UnitOfWork(ApplicationDbContext context, ITaskDefinitionRepository taskDefinitionRepository,
    IJobRepository jobRepository, ITaskActivityRepository taskActivityRepository,
    ITaskWeekRepository taskWeekRepository, IFundRepository fundRepository,
    ITransactionLogRepository transactionLogRepository, IUserRepository userRepository, ILogger<IUnitOfWork> logger)
        : IUnitOfWork
{
    private ApplicationDbContext Context { get; set; } = context;
    public ITaskDefinitionRepository TaskDefinitionRepository { get; private set; } = taskDefinitionRepository;
    public IJobRepository JobRepository { get; private set; } = jobRepository;
    public ITaskActivityRepository TaskActivityRepository { get; private set; } = taskActivityRepository;
    public ITaskWeekRepository TaskWeekRepository { get; private set; } = taskWeekRepository;
    public IFundRepository FundRepository { get; private set; } = fundRepository;
    public ITransactionLogRepository TransactionLogRepository { get; private set; } = transactionLogRepository;
    public IUserRepository UserRepository { get; private set; } = userRepository;
    public ILogger Logger { get; private set; } = logger;

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
    public async Task AddUser(ApplicationUser user)
    {
        Guard.IsNotNull(user);
        Guard.IsNotNull(user.NewPassword);
        await UserRepository.Add(user);
    }   
    public async Task DeleteUser(ApplicationUser user)
    {
        Guard.IsNotNull(user);
        await UserRepository.Delete(user);
    }
    public async Task<string> UpdateUser(ApplicationUser user)
    {
        Guard.IsNotNull(user);
        await UserRepository.Update(user);
        if(!String.IsNullOrEmpty(user.NewPassword)) return await UserRepository.ResetPassword(user);
        return string.Empty;

    }
    public async Task<String> ResetPassword(ApplicationUser user)
    {
        Guard.IsNotNull(user);
        Guard.IsNotNull(user.NewPassword);
        return await UserRepository.ResetPassword(user);
    }
}
