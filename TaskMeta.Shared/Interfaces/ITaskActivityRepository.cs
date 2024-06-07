
using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Interfaces;
public interface ITaskActivityRepository : IRepositoryBase<TaskActivity>
{
    Task<List<TaskActivity>> GetListByTaskWeek(TaskWeek taskWeek);
    Task Add(List<TaskActivity> taskActivityList);
    Task<List<TaskActivity>> GetListByDate(DateOnly date, ApplicationUser user);
    Task CreateForWeek(TaskWeek taskWeek, List<TaskDefinition> taskDefinitionList);
}