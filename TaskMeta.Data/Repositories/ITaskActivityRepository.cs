using TaskMeta.Data.Models;
namespace TaskMeta.Data.Repositories;
public interface ITaskActivityRepository
{
    Task AddAsync(TaskActivity taskActivity);
    Task DeleteAsync(int id);
    Task<List<TaskActivity>> GetAllAsync();
    Task<TaskActivity> GetByIdAsync(int id);
    Task UpdateAsync(TaskActivity taskActivity);
    Task<List<TaskActivity>> GetByDate(DateOnly date);
}