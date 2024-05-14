using TaskMeta.Data.Models;
using TaskMeta.Data.Repositories;

namespace TaskMeta.Data.Services;
public class TaskDefinitionService : ITaskDefinitionService
{
    private readonly ITaskDefinitionRepository _taskDefinitionRepository;

    public TaskDefinitionService(ITaskDefinitionRepository taskDefinitionRepository)
    {
        _taskDefinitionRepository = taskDefinitionRepository;
    }

    public async Task<List<TaskDefinition>> GetAllAsync()
    {
        return await _taskDefinitionRepository.GetAllAsync();
    }

    public async Task<TaskDefinition> GetByIdAsync(int id)
    {
        return await _taskDefinitionRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(TaskDefinition taskDefinition)
    {
        // Add any business logic or validation here
        await _taskDefinitionRepository.AddAsync(taskDefinition);
    }

    public async Task UpdateAsync(TaskDefinition taskDefinition)
    {
        // Add any business logic or validation here
        await _taskDefinitionRepository.UpdateAsync(taskDefinition);
    }

    public async Task DeleteAsync(int id)
    {
        // Add any business logic or validation here
        await _taskDefinitionRepository.DeleteAsync(id);
    }
}
