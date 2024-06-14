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
        :   IUnitOfWork
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

    public async Task SaveChanges()
    {
        await Context.SaveChangesAsync();
    }

    
    public async Task<TaskWeek> GetOrCreateCurrentWeek(ApplicationUser user)
    {
        Guard.IsNotNull(user);

        var currentWeekStart = Tools.StartOfWeek(DateTime.Now);

        var currentWeek = await TaskWeekRepository.Get(user.Id, currentWeekStart);
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
            await TaskActivityRepository.CreateForWeek(currentWeek, await TaskDefinitionRepository.GetList());
       
        }
        currentWeek.User = user;

        return currentWeek;
    }
    // Performs the necessary operations to set finalize the status of the task week
    /// </summary>
    /// <param name="taskWeek">The TaskWeek object to accept.</param>
    /// <param name="commit">A flag indicating whether to commit the changes to the database. Default is true.</param>
    public async Task AcceptWeek(TaskWeek taskWeek)
    {
        // Get the list of funds associated with the user
        List<Fund> fundList = await FundRepository.GetFundsByUser(taskWeek.UserId);

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

    }
    /// <summary>
    /// Processes a transaction by updating the balances of the associated funds.
    /// </summary>
    /// <param name="transaction">The transaction to be processed.</param>
    public void Process(Transaction transaction)
    {
        switch (transaction.CategoryId)
        {
            case Constants.Category.Deposit:
                {
                    Deposit(transaction);
                    break;
                }
            case Constants.Category.Withdrawal:
                {
                    Withdraw(transaction);
                    break;
                }
            case Constants.Category.Transfer:
                {
                    Deposit(transaction);
                    Withdraw(transaction);
                    break;
                }
        }
    }

    private void Withdraw(Transaction transaction)
    {
        var sourceFund = transaction.SourceFund!;
        if (sourceFund.Balance < transaction.Amount)
        {
            throw new InvalidOperationException("Insufficient funds.");
        }
        transaction.PreviousAmount = sourceFund.Balance;
        sourceFund.Balance -= transaction.Amount;
        TransactionLogRepository!.LogTransaction(transaction);
    }

    private void Deposit(Transaction transaction)
    {
        var targetFund = transaction.TargetFund!;
        transaction.PreviousAmount = targetFund.Balance;
        targetFund.Balance += transaction.Amount;
        TransactionLogRepository!.LogTransaction(transaction);
    }
}
