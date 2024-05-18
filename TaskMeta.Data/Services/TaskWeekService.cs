using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Data.Services
{
    public class TaskWeekService : EntityService<TaskWeek>, ITaskWeekService
    {

        public TaskWeekService(ApplicationDbContext applicationDbContext, IUserService userService,
            ILogger<EntityService<TaskWeek>> logger)
        : base(applicationDbContext, userService, logger)
        {
        }
        public async Task<TaskWeek> GetOrCreateCurrentWeek(string userId)
        {
            var currentWeekStart = Tools.StartOfWeek;
            return await GetOrCreate(userId, currentWeekStart);
        }

        public async Task<TaskWeek> GetOrCreate(string userId, DateOnly currentWeekStart)
        { 
            var currentWeek = await Get(userId, currentWeekStart);
            if (currentWeek == null)
            {
                currentWeek = new TaskWeek()
                {
                    WeekStartDate = Tools.StartOfWeek,
                    UserId = userId,
                    StatusId = 1,
                    Value = 0
                };
                await AddAsync(currentWeek);
            }
            return currentWeek;
        }

        // generate comments for this method

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
    }
}
