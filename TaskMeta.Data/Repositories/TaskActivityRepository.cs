using CommunityToolkit.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskMeta.Shared.Models.Repositories;
public class TaskActivityRepository(ApplicationDbContext applicationDbContext, ICacheProvider cacheProvider, ILogger<TaskActivity> logger)
    : RepositoryBase<TaskActivity>(applicationDbContext, cacheProvider, logger), ITaskActivityRepository
{


    /// <summary>
    /// Adds a list of TaskActivities asynchronously.
    /// </summary>
    /// <param name="taskActivityList">The list of TaskActivities to add.</param>
    public async Task Add(List<TaskActivity> taskActivityList)
    {
        try
        {
            await Context.TaskActivities.AddRangeAsync([.. taskActivityList]);

        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while adding a set of TaskActivities");
            throw;
        }
    }
    public async Task CreateForWeek(TaskWeek taskWeek, List<TaskDefinition> taskDefinitionList)
    {

        for (int day = 0; day < 7; day++)
        {
            var currentDate = taskWeek.WeekStartDate.AddDays(day);

            List<TaskActivity>? list;



            list = [];
            foreach (var taskDefinition in taskDefinitionList)
            {
                TaskActivity taskActivity = new()
                {
                    Sequence = taskDefinition.Sequence,
                    TaskDefinitionId = taskDefinition.Id,
                    TaskDate = currentDate,
                    Complete = false,
                    Value = taskDefinition.Value,
                    Description = taskDefinition.Description,
                    TaskWeek = taskWeek
                };
                list.Add(taskActivity);
            }

            await Add(list);

        }
    }

    public async Task<List<TaskActivity>> GetListByTaskWeek(TaskWeek taskWeek)
    {
        List<TaskActivity>? list = null;

        if (taskWeek != null)
        {
            try
            {
                var set = Context.TaskActivities.Where(t => t.TaskWeekId == taskWeek.Id);
                list = await set.OrderBy(t => t.Sequence).OrderBy(t => t.TaskDate).ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while retrieving TaskActivities by task week");
                throw;
            }
        }

        return list!;
    }

    /// <summary>
    /// Retrieves a list of TaskActivities for a specific date and user.
    /// </summary>
    /// <param name="date">The date to filter the TaskActivities.</param>
    /// <param name="user">The user to filter by.</param>
    /// <returns>A list of TaskActivities filtered by date and user.</returns>
    public async Task<List<TaskActivity>> GetListByDate(DateOnly date, ApplicationUser user)
    {
        Guard.IsNotNull(user);
        try
        {
            var set = Context.TaskActivities
                .Where(t => t.TaskDate == date && t.TaskWeek.UserId == user.Id);
            return await set.OrderBy(t => t.Sequence).ToListAsync();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while retrieving TaskActivities by date and user");
            throw;
        }
    }
}
