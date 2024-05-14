using TaskMeta.Data.Models;
using TaskMeta.Data.Repositories;

namespace TaskMeta.Data.Services;
public class TaskActivityService : ITaskActivityService
{
    private readonly ITaskActivityRepository _taskActivityRepository;
    private readonly ITaskDefinitionRepository _taskDefinitionRepository;

    public TaskActivityService(ITaskActivityRepository taskActivityRepository, ITaskDefinitionRepository taskDefinitionRepository)
    {
        _taskActivityRepository = taskActivityRepository;
        _taskDefinitionRepository = taskDefinitionRepository;
    }

    public async Task<List<TaskActivity>> GetAllAsync()
    {
        return await _taskActivityRepository.GetAllAsync();
    }
    public async Task<List<TaskActivity>> GetByDate(DateOnly date)
    {
        var list = await _taskActivityRepository.GetByDate(date);
        List<TaskActivity> result = list.OrderBy(t => t.Sequence).ToList();
     
        if (result == null || result.Count == 0)
        {
            result = new List<TaskActivity>();
            List<TaskDefinition> taskDefinitions = await _taskDefinitionRepository.GetAllAsync();
            foreach (var taskDefinition in taskDefinitions)
            {
                TaskActivity taskActivity = new TaskActivity
                {
                    Sequence = taskDefinition.Sequence,
                    TaskDefinitionId = taskDefinition.Id,
                    TaskDate = date,
                    Complete = false,
                    Description = taskDefinition.Description
                };
                result.Add(taskActivity);
            }

        }
        return result;
    }
    public async Task<TaskActivity> GetByIdAsync(int id)
    {
        return await _taskActivityRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(TaskActivity taskActivity)
    {
        // Add any business logic or validation here
        await _taskActivityRepository.AddAsync(taskActivity);
    }

    public async Task UpdateAsync(TaskActivity taskActivity)
    {
        // Add any business logic or validation here
        await _taskActivityRepository.UpdateAsync(taskActivity);
    }

    public async Task DeleteAsync(int id)
    {
        // Add any business logic or validation here
        await _taskActivityRepository.DeleteAsync(id);
    }
}
