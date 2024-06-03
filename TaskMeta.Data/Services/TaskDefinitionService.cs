using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;

namespace TaskMeta.Data.Services;
public class TaskDefinitionService(ApplicationDbContext applicationDbContext, IUserService userService,
    ILogger<EntityService<TaskDefinition>> logger) :
    EntityService<TaskDefinition>(applicationDbContext, userService, logger), ITaskDefinitionService
{
    /// <summary>
    /// Retrieves the queryable collection of task definitions.
    /// </summary>
    /// <returns>The queryable collection of task definitions.</returns>
    public async Task<List<TaskDefinition>> GetList()
    {
        try
        {
            var result = await Context.TaskDefinitions
                .Include(t => t.User)
                .OrderBy(t => t.Sequence)
                .ToListAsync();
            return result;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"An error occurred while getting task definition query.");
            throw;
        }
    }

    /// <summary>
    /// Retrieves the queryable collection of task definitions filtered by user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The queryable collection of task definitions filtered by user.</returns>
    public async Task<List<TaskDefinition>> GetListByUser(string userId)
    {
        try
        {
            var result = await Context.TaskDefinitions
                .Include(t => t.User)
                .Where(t => t.UserId == userId || t.UserId == null)
                .OrderBy(t => t.Sequence)
                .ToListAsync();
            return result;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"An error occurred while getting task definition query.");
            throw;
        }
    }
}
