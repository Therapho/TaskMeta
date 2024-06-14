using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Models.Repositories;

public class TaskWeekRepository(ApplicationDbContext applicationDbContext, ICacheProvider cacheProvider, ILogger<TaskWeek> logger) :
    RepositoryBase<TaskWeek>(applicationDbContext, cacheProvider, logger), ITaskWeekRepository
{


    public void UpdateValue(TaskWeek taskWeek, decimal valueChange, bool add)
    {
        taskWeek.Value = add ? taskWeek.Value + valueChange : taskWeek.Value - valueChange;
        Update(taskWeek);

    }
    /// <summary>
    /// Retrieves a TaskWeek object based on the provided user ID and week start date.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="weekStart">The start date of the week.</param>
    /// <returns>The TaskWeek object if found, otherwise null.</returns>
    public async Task<TaskWeek?> Get(string userId, DateOnly weekStart)
    {
        var key = Key("U", userId, "D", weekStart);
        var result = CacheProvider.Get<TaskWeek>(key);
        if (result != null) return result;

        var query = Context.Set<TaskWeek>()
            .Where(x => x.WeekStartDate == weekStart && x.UserId == userId)
            .Include(w => w.User);

        result = await query.FirstOrDefaultAsync();
        CacheProvider.Set(key, result, 10);

        return result;
    }
    public async Task<(TaskWeek? previousWeek, TaskWeek? nextWeek)> GetAdjacent(TaskWeek currentTaskWeek)
    {
        DateOnly previousStartDate = currentTaskWeek.WeekStartDate.AddDays(-7);
        DateOnly nextStartDate = currentTaskWeek.WeekStartDate.AddDays(7);

        var previousWeek = await Get(currentTaskWeek.UserId, previousStartDate);
        var nextWeek = await Get(currentTaskWeek.UserId, nextStartDate);
        return (previousWeek, nextWeek);
    }

    public async Task<List<TaskActivity>?> GetByDate(TaskWeek taskWeek, DateOnly dateOnly)
    {
        var key = ListKey("TW", taskWeek, "D", dateOnly);
        var result = CacheProvider.Get<List<TaskActivity>>(key);
        if (result != null) return result;

        var query = Context.Set<TaskActivity>()
            .Where(t => t.TaskWeekId == taskWeek.Id && t.TaskDate == dateOnly);
        result = await query.ToListAsync();
        CacheProvider.Set(key, result, 10);

        return result;
    }
}
