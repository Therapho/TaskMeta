using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;

namespace TaskMeta.Data.Services;
public class TaskActivityService : EntityService<TaskActivity>, ITaskActivityService
{
    public ITaskDefinitionService TaskDefinitionService { get; set; }

    public TaskActivityService(ITaskDefinitionService taskDefinitionService, ApplicationDbContext applicationDbContext, 
        IUserService userService, ILogger<EntityService<TaskActivity>> logger)
        : base(applicationDbContext, userService, logger) 
    {
        TaskDefinitionService = taskDefinitionService;
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

            //Logger.LogError(ex, "An error occurred while adding a set of TaskActivities");
            throw;
        }
    }
    public async Task<List<TaskActivity>> GetOrCreateTaskActivities(TaskWeek taskWeek)
    {
        var list = await GetByTaskWeek(taskWeek);

        if (list == null || list.Count == 0)
        {
            list = new List<TaskActivity>();
            List<TaskDefinition> taskDefinitions = await TaskDefinitionService.GetAllAsync();

            var currentDate = taskWeek!.WeekStartDate;

            foreach (var taskDefinition in taskDefinitions)
            {
                TaskActivity taskActivity = new TaskActivity
                {
                    Sequence = taskDefinition.Sequence,
                    TaskDefinitionId = taskDefinition.Id,
                    TaskDate = currentDate,
                    Complete = false,
                    Description = taskDefinition.Description,
                    TaskWeek = taskWeek
                };
                list.Add(taskActivity);
                currentDate = currentDate.AddDays(1);
            }
            await AddAsync(list);
        }
        return list;
    }
    public async Task<List<TaskActivity>> GetByTaskWeek(TaskWeek taskWeek)
    {
        List<TaskActivity>? list = null;

        if(taskWeek != null)
        {
            var set = Context.TaskActivities.Where(t=>t.TaskWeekId == taskWeek.Id);
            list = await set.OrderBy(t => t.Sequence).ToListAsync();
        }

        return list!;
    }
}
