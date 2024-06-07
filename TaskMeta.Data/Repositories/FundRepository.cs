using Microsoft.Extensions.Logging;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;

namespace TaskMeta.Data.Repositories;

public class FundRepository(ApplicationDbContext applicationDbContext, ILogger<Fund> logger) :
    RepositoryBase<Fund>(applicationDbContext, logger), IFundRepository
{

    /// <summary>
    /// Retrieves a list of funds associated with a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of funds.</returns>
    public Task<List<Fund>> GetFundsByUser(string userId)
    {
        try
        {
            return Task.FromResult(Context.Funds.Where(f => f.UserId == userId).ToList());
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"An error occurred while getting funds by user {userId}.");
            throw;
        }
    }


}
