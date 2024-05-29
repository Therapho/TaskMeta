
using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Interfaces;
public interface ITaskActivityService : IEntityService<TaskActivity>
{
    Task<List<TaskActivity>> GetByTaskWeek(TaskWeek taskWeek);
    Task<List<TaskActivity>> GetOrCreateTaskActivities(TaskWeek taskWeek, DateOnly date, bool commit = true);
    Task AddAsync(List<TaskActivity> taskActivityList, bool commit);
    Task<List<TaskActivity>> GetByDate(DateOnly date, string userId);


}