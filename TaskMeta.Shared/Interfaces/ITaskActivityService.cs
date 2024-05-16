
using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Interfaces;
public interface ITaskActivityService : IEntityService<TaskActivity>
{
    Task<List<TaskActivity>> GetByTaskWeek(TaskWeek taskWeek);
    Task<List<TaskActivity>> GetOrCreateTaskActivities(TaskWeek taskWeek);
    Task AddAsync(List<TaskActivity> taskActivityList);

}