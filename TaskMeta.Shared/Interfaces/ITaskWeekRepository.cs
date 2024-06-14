using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Interfaces
{
    public interface ITaskWeekRepository : IRepositoryBase<TaskWeek>
    {
        
        TaskWeek? Get(string userId, DateOnly weekStart);
        (TaskWeek? previousWeek, TaskWeek? nextWeek) GetAdjacent(TaskWeek currentTaskWeek);
        void UpdateValue(TaskWeek task, decimal valueChange, bool add);
        
    }
}
