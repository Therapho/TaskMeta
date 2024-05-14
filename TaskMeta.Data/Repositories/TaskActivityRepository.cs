using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskMeta.Data.Models;

namespace TaskMeta.Data.Repositories;
public class TaskActivityRepository : ITaskActivityRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TaskActivityRepository> _logger;

    public TaskActivityRepository(ApplicationDbContext context, ILogger<TaskActivityRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<TaskActivity>> GetAllAsync()
    {
        try
        {
            return await _context.TaskActivities.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting TaskActivities");
            throw;
        }
    }
    public async Task<List<TaskActivity>> GetByDate(DateOnly date)
    {
        try
        {
            var activities = _context.TaskActivities;

            return await activities.Where(ta => ta.TaskDate == date).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting TaskActivities for date {date}");
            throw;
        }
    }
    public async Task<TaskActivity?> GetByIdAsync(int id)
    {
        try
        {
            return await _context.TaskActivities.FindAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting TaskActivity with id {id}");
            throw;
        }
    }
    public async Task AddAsync(List<TaskActivity> taskActivityList)
    {
        try 
        {
            await _context.TaskActivities.AddRangeAsync(taskActivityList.ToArray());
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "An error occurred while adding a set of TaskActivities");
            throw;
        }
    }
    public async Task AddAsync(TaskActivity taskActivity)
    {
        try
        {
            await _context.TaskActivities.AddAsync(taskActivity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding a TaskActivity");
            throw;
        }
    }

    public async Task UpdateAsync(TaskActivity taskActivity)
    {
        try
        {
            _context.TaskActivities.Update(taskActivity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while updating TaskActivity with id {taskActivity.Id}");
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var taskActivity = await _context.TaskActivities.FindAsync(id);
            if (taskActivity != null)
            {
                _context.TaskActivities.Remove(taskActivity);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while deleting TaskActivity with id {id}");
            throw;
        }
    }
}
