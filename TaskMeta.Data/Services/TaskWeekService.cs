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
            var currentWeekStart = DateOnly.FromDateTime(DateTime.Now.StartOfWeek());

            var currentWeek = await Get(userId, currentWeekStart);
            if (currentWeek == null)
            {
                currentWeek = new TaskWeek()
                {
                    WeekStartDate = DateOnly.FromDateTime(DateTime.Now.StartOfWeek()),
                    UserId = userId,
                    StatusId = 1,
                    Value = 0
                };
                await AddAsync(currentWeek);
            }
            return currentWeek;
        }
        public async Task<TaskWeek?> Get(string userId, DateOnly weekStart)
        {

            var currentWeek = await Context.Set<TaskWeek>()
                .Where(x => x.WeekStartDate == weekStart && x.UserId == userId).FirstOrDefaultAsync();

            return currentWeek;
        }
    }
}
