using TaskMeta.Data.Models;

namespace TaskMeta.Data.Services;
public interface ITaskActivityService
{
    Task AddAsync(TaskActivity taskActivity);
    Task DeleteAsync(int id);
    Task<List<TaskActivity>> GetAllAsync();
    Task<List<TaskActivity>> GetByDate(DateOnly date);

    Task<TaskActivity> GetByIdAsync(int id);
    Task UpdateAsync(TaskActivity taskActivity);
}