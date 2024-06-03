using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Data.Services;
public class TaskActivityService(ITaskDefinitionService taskDefinitionService, ApplicationDbContext applicationDbContext,
    IUserService userService, ILogger<EntityService<TaskActivity>> logger) : EntityService<TaskActivity>(applicationDbContext, userService, logger), ITaskActivityService
{
    public ITaskDefinitionService TaskDefinitionService { get; set; } = taskDefinitionService;

    /// <summary>
    /// Adds a list of TaskActivities asynchronously.
    /// </summary>
    /// <param name="taskActivityList">The list of TaskActivities to add.</param>
    public async Task AddAsync(List<TaskActivity> taskActivityList, bool commit = true)
    {
        try
        {
            await Context.TaskActivities.AddRangeAsync([.. taskActivityList]);
            if (commit) await Commit();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while adding a set of TaskActivities");
            throw;
        }
    }
  
    public async Task<List<TaskActivity>> GetOrCreateTaskActivities(TaskWeek taskWeek, DateOnly date, bool commit = true)
    {
        List<TaskActivity>? list;
        try
        {
            list = await GetByDate(date, taskWeek.UserId);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while adding a set of TaskActivities");
            throw;
        }       

        if (list == null || list.Count == 0)
        {
            list = [];
            List<TaskDefinition> taskDefinitions = await TaskDefinitionService.GetListByUser(taskWeek.UserId);

            foreach (var taskDefinition in taskDefinitions)
            {
                TaskActivity taskActivity = new()
                {
                    Sequence = taskDefinition.Sequence,
                    TaskDefinitionId = taskDefinition.Id,
                    TaskDate = date,
                    Complete = false,
                    Value = taskDefinition.Value,
                    Description = taskDefinition.Description,
                    TaskWeek = taskWeek
                };
                list.Add(taskActivity);
            }

            await AddAsync(list, commit);
        }
        return list;
    }

    public async Task<List<TaskActivity>> GetByTaskWeek(TaskWeek taskWeek)
    {
        List<TaskActivity>? list = null;

        if (taskWeek != null)
        {
            var set = Context.TaskActivities.Where(t => t.TaskWeekId == taskWeek.Id);
            list = await set.OrderBy(t => t.Sequence).OrderBy(t => t.TaskDate).ToListAsync();
        }

        return list!;
    }

    /// Retrieves a list of TaskActivities for a specific date and user.
    /// </summary>
    /// <param name="date">The date to filter the TaskActivities.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A list of TaskActivities filtered by date and user.</returns>
    public async Task<List<TaskActivity>> GetByDate(DateOnly date, string userId)
    {
        try
        {
            var set = Context.TaskActivities
                .Where(t => t.TaskDate == date && t.TaskWeek.UserId == userId);
            return await set.OrderBy(t => t.Sequence).ToListAsync();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while retrieving TaskActivities by date and user");
            throw;
        }
    }
}
