using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;

namespace TaskMeta.Data.Services;

public class FundService : EntityService<Fund>, IFundService
{

    public FundService(ApplicationDbContext applicationDbContext, IUserService userService, ILogger<EntityService<Fund>> logger)
        : base(applicationDbContext, userService, logger) { }

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
