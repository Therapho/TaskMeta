using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskMeta.Data.Models;

namespace TaskMeta.Data.Repositories;
public class TaskDefinitionRepository : ITaskDefinitionRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TaskDefinitionRepository> _logger;

    public TaskDefinitionRepository(ApplicationDbContext context, ILogger<TaskDefinitionRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<TaskDefinition>> GetAllAsync()
    {
        try
        {
            return await _context.TaskDefinitions.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting TaskDefinitions");
            throw;
        }
    }

    public async Task<TaskDefinition> GetByIdAsync(int id)
    {
        try
        {
            return await _context.TaskDefinitions.FindAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting TaskDefinition with id {id}");
            throw;
        }
    }

    public async Task AddAsync(TaskDefinition taskDefinition)
    {
        try
        {
            await _context.TaskDefinitions.AddAsync(taskDefinition);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding a TaskDefinition");
            throw;
        }
    }

    public async Task UpdateAsync(TaskDefinition taskDefinition)
    {
        try
        {
            _context.TaskDefinitions.Update(taskDefinition);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while updating TaskDefinition with id {taskDefinition.Id}");
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var taskDefinition = await _context.TaskDefinitions.FindAsync(id);
            if (taskDefinition != null)
            {
                _context.TaskDefinitions.Remove(taskDefinition);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while deleting TaskDefinition with id {id}");
            throw;
        }
    }
}
