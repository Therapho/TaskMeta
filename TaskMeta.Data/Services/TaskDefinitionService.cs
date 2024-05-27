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
    public IQueryable<TaskDefinition> GetTaskDefinitionsQuery()
    {
        try
        {
            return Context.TaskDefinitions.AsQueryable<TaskDefinition>()
                .Include(t=>t.User);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"An error occurred while getting task definition query.");
            throw;
        }
    }
}
