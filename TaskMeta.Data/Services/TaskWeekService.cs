using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System.Security;
using TaskMeta.Shared;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Data.Services
{
    public class TaskWeekService : EntityService<TaskWeek>, ITaskWeekService
    {
        private IFundService _fundService;
        private ITransactionLogService _transactionLogService;

        public TaskWeekService(ApplicationDbContext applicationDbContext, IFundService fundService,
            ITransactionLogService transactionLogService, IUserService userService,ILogger<EntityService<TaskWeek>> logger)
        : base(applicationDbContext, userService, logger)
        {
            _fundService = fundService;
            _transactionLogService = transactionLogService;
        }
        public async Task<TaskWeek> GetOrCreateCurrentWeek(string userId)
        {
            var currentWeekStart = Tools.StartOfWeek(DateTime.Now);
            return await GetOrCreate(userId, currentWeekStart);
        }

        public async Task<TaskWeek> GetOrCreate(string userId, DateOnly currentWeekStart, bool commit = true)
        { 
            var currentWeek = await Get(userId, currentWeekStart);
            if (currentWeek == null)
            {
                currentWeek = new TaskWeek()
                {
                    WeekStartDate = Tools.StartOfWeek(DateTime.Now),
                    UserId = userId,
                    StatusId = 1,
                    Value = 0
                };
                await AddAsync(currentWeek, commit);
            }
            return currentWeek;
        }

        /// <summary>
        /// Retrieves a TaskWeek object based on the provided user ID and week start date.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="weekStart">The start date of the week.</param>
        /// <returns>The TaskWeek object if found, otherwise null.</returns>
        public async Task<TaskWeek?> Get(string userId, DateOnly weekStart)
        {

            var currentWeek = await Context.Set<TaskWeek>()
                .Where(x => x.WeekStartDate == weekStart && x.UserId == userId).FirstOrDefaultAsync();

            return currentWeek;
        }
        public async Task<(TaskWeek? previousWeek, TaskWeek? nextWeek)> GetAdjacent(TaskWeek currentTaskWeek)
        {
            DateOnly previousStartDate = currentTaskWeek.WeekStartDate.AddDays(-7);
            DateOnly nextStartDate = currentTaskWeek.WeekStartDate.AddDays(7);
            
            var previousWeek = await Get(currentTaskWeek.UserId, previousStartDate);
            var nextWeek = await Get(currentTaskWeek.UserId, nextStartDate);
            return (previousWeek, nextWeek);
        }
        
        
        // Performs the necessary operations to set finalize the status of the task week
        /// </summary>
        /// <param name="taskWeek">The TaskWeek object to accept.</param>
        /// <param name="commit">A flag indicating whether to commit the changes to the database. Default is true.</param>
        public async Task AcceptWeek(TaskWeek taskWeek, bool commit = true)
        {
            var currentUser = await UserService.GetCurrentUser()!;
            
            // Get the list of funds associated with the user
            List<Fund> fundList = await _fundService.GetFundsByUser(taskWeek.UserId);

            // Iterate through each fund and perform the necessary operations
            foreach (var fund in fundList)
            {
                // Calculate the deposit amount based on the task week value and fund allocation
                var depositAmount = taskWeek.Value * fund.Allocation.GetValueOrDefault() / 100;


               

                // Create a transaction log for the deposit
                TransactionLog transactionLog = new TransactionLog
                {
                    SourceFundId = null,
                    TargetFundId = fund.Id,
                    Amount = depositAmount,
                    PreviousAmount = fund.Balance,
                    CategoryId = Constants.Category.Deposit,
                    CallingUserId = currentUser.Id,
                    TargetUserId = taskWeek.UserId,
                    Date = DateTime.Now,
                    Description = "Weekly deposit from accepted tasks."
                };
                await _transactionLogService.AddAsync(transactionLog, false);

                // Update the fund balance by adding the deposit amount
                fund.Balance += depositAmount;
                await _fundService.UpdateAsync(fund, false);
            }

            // Update the status of the task week to "Accepted"
            taskWeek.StatusId = 2;
            await UpdateAsync(taskWeek, false);

            // Commit the changes to the database if specified
            if (commit) await Commit();
        }
    }
}
