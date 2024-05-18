using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Interfaces
{
    public interface ITaskWeekService : IEntityService<TaskWeek>
    {
        Task<TaskWeek> GetOrCreateCurrentWeek(string userId);
        Task<TaskWeek?> Get(string userId, DateOnly weekStart);
        Task<(TaskWeek? previousWeek, TaskWeek? nextWeek)> GetAdjacent(TaskWeek currentTaskWeek);
    }
}
