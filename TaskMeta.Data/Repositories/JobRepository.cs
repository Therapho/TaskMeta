using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Shared.Models.Repositories;


public class JobRepository(ApplicationDbContext applicationDbContext, ICacheProvider cacheProvider,
    ILogger<Job> logger) : RepositoryBase<Job>(applicationDbContext, cacheProvider, logger), IJobRepository
{


    /// <summary>
    /// Retrieves a list of jobs that are not overdue or completed in previous weeks
    /// </summary>
    /// <returns>A list of incomplete jobs.</returns>
    public List<Job> GetCurrentJobs()
    {
        try
        {
            Logger.LogInformation("Retrieving current jobs...");
            var endOfWeek = DateTime.Now.EndOfWeek();

            var query = Context.Jobs.Where(j => j.Complete == false);
            return query.ToList();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred while retrieving incomplete jobs.");
            throw;
        }
    }
    /// <summary>
    /// Retrieves a list of jobs that are not marked complete.
    /// </summary>
    /// <returns>A list of incomplete jobs.</returns>
    public List<Job> GetCurrentJobs(ApplicationUser user)
    {
        try
        {
            Logger.LogInformation($"Retrieving current jobs for user...{user.UserName}");
            var endOfWeek = DateTime.Now.EndOfWeek();
            var startOfWeek = DateTime.Now.StartOfWeek();
            var today = DateTime.Now.ToDateOnly();

            var query = Context.Jobs.Where
                (j => 
                    (
                        (j.DateCompleted >= startOfWeek && j.DateCompleted <= endOfWeek) || j.DateCompleted == null
                    ) 
                    && j.DateDue <= today && j.UserId == user.Id
            );
            return query.ToList();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred while retrieving incomplete jobs.");
            throw;
        }
    }
}