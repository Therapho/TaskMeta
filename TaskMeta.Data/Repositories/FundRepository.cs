using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Models.Repositories;

public class FundRepository(ApplicationDbContext applicationDbContext, ICacheProvider cacheProvider, ILogger<Fund> logger) :
    RepositoryBase<Fund>(applicationDbContext, cacheProvider, logger), IFundRepository
{

    /// <summary>
    /// Retrieves a list of funds associated with a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of funds.</returns>
    public async Task<List<Fund>> GetFundsByUser(string userId)
    {
        try
        {
            return await Context.Funds.Where(f => f.UserId == userId).ToListAsync();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"An error occurred while getting funds by user {userId}.");
            throw;
        }
    }


}
