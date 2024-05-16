using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskMeta.Data.Models;

namespace TaskMeta.Data.Repositories;
public class TaskActivityRepository : EntityRepository<TaskActivity>, ITaskActivityRepository
{
    public TaskActivityRepository(ApplicationDbContext context, ILogger<EntityRepository<TaskActivity>> logger) : base(context, logger)
    {
    }
    public async Task AddAsync(List<TaskActivity> taskActivityList)
    {
        try 
        {
            await Context.TaskActivities.AddRangeAsync(taskActivityList.ToArray());
            await Context.SaveChangesAsync();
        }
        catch (Exception ex)
        {

            Logger.LogError(ex, "An error occurred while adding a set of TaskActivities");
            throw;
        }
    }
    public async Task<List<TaskActivity>> GetByDate(DateOnly date)
    {
        try
        {
            var activities = Context.TaskActivities;

            return await activities.Where(ta => ta.TaskDate == date).ToListAsync();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"An error occurred while getting TaskActivities for date {date}");
            throw;
        }
    }
}
