
using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Interfaces;
public interface ITaskActivityRepository : IRepositoryBase<TaskActivity>
{
    List<TaskActivity> GetListByTaskWeek(TaskWeek taskWeek);
    void Add(List<TaskActivity> taskActivityList);
    List<TaskActivity> GetListByDate(DateOnly date, ApplicationUser user);
    void CreateForWeek(TaskWeek taskWeek, List<TaskDefinition> taskDefinitionList);
}