using CommunityToolkit.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using System.Diagnostics;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Data.Services;


public class JobService(ApplicationDbContext applicationDbContext, IUserService userService,
    ILogger<EntityService<Job>> logger) : EntityService<Job>(applicationDbContext, userService, logger), IJobService
{

    
    /// <summary>
    /// Retrieves a list of jobs that are not overdue or completed in previous weeks
    /// </summary>
    /// <returns>A list of incomplete jobs.</returns>
    public async Task<List<Job>> GetCurrentJobs()
    {
        try
        {
            Logger.LogInformation("Retrieving current jobs...");
            var endOfWeek = DateTime.Now.EndOfWeek();

            var query = Context.Jobs.Where(j => j.Complete == false );
            return await query.ToListAsync();
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
    public async Task<List<Job>> GetCurrentJobs(ApplicationUser user)
    {
        try
        {
            Logger.LogInformation($"Retrieving current jobs for user...{user.UserName}");
            var endOfWeek = DateTime.Now.EndOfWeek();
            var startOfWeek = DateTime.Now.StartOfWeek();

            var query = Context.Jobs.Where(j => (j.DateCompleted >= startOfWeek || j.DateCompleted == null) && j.DateDue < endOfWeek && j.UserId == user.Id);
            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred while retrieving incomplete jobs.");
            throw;
        }
    }
}