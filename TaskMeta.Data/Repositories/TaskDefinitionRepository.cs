using CommunityToolkit.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Models.Repositories;
public class TaskDefinitionRepository(ApplicationDbContext applicationDbContext, ICacheProvider cacheProvider,
    ILogger<TaskDefinition> logger) :
    RepositoryBase<TaskDefinition>(applicationDbContext, cacheProvider, logger), ITaskDefinitionRepository
{
    /// <summary>
    /// Retrieves the queryable collection of task definitions.
    /// </summary>
    /// <returns>The queryable collection of task definitions.</returns>
    public List<TaskDefinition> GetList()
    {
        try
        {
            var result = Context.TaskDefinitions
                .Include(t => t.User)
                .OrderBy(t => t.Sequence)
                .ToList();
            return result;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"An error occurred while getting task definition query.");
            throw;
        }
    }

    public List<TaskDefinition> GetList(ApplicationUser user)
    {
        try
        {
            var result = Context.TaskDefinitions
                .Where(t => t.UserId == user.Id || t.UserId == null)
                .Include(t => t.User)
                .OrderBy(t => t.Sequence)
                .ToList();
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
    public List<TaskDefinition> GetListByUser(ApplicationUser user)
    {
        Guard.IsNotNull(user);
        try
        {
            var result = Context.TaskDefinitions
                .Include(t => t.User)
                .Where(t => t.UserId == user.Id || t.UserId == null)
                .OrderBy(t => t.Sequence)
                .ToList();
            return result;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"An error occurred while getting task definition query.");
            throw;
        }
    }
}
