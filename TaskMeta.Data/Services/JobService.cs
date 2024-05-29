using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;

namespace TaskMeta.Data.Services;


public class JobService(ApplicationDbContext applicationDbContext, IUserService userService,
    ILogger<EntityService<Job>> logger) : EntityService<Job>(applicationDbContext, userService, logger), IJobService
{

    /// <summary>
    /// Retrieves a list of jobs that are not marked complete.
    /// </summary>
    /// <returns>A list of incomplete jobs.</returns>
    public async Task<List<Job>> GetIncompleteJobs()
    {
        try
        {
            Logger.LogInformation("Retrieving incomplete jobs...");

            var query = Context.Jobs.Where(j => j.Complete == false);
            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred while retrieving incomplete jobs.");
            throw;
        }
    }
}