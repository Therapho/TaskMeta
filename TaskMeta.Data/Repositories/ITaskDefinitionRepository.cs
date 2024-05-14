using TaskMeta.Data.Models;
namespace TaskMeta.Data.Repositories;
public interface ITaskDefinitionRepository
{
    Task AddAsync(TaskDefinition taskDefinition);
    Task DeleteAsync(int id);
    Task<List<TaskDefinition>> GetAllAsync();
    Task<TaskDefinition> GetByIdAsync(int id);
    Task UpdateAsync(TaskDefinition taskDefinition);
}