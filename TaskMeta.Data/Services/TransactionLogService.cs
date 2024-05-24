using Microsoft.Extensions.Logging;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;

namespace TaskMeta.Data.Services
{
    public class TransactionLogService : EntityService<TransactionLog>, ITransactionLogService
    {
        public TransactionLogService(ApplicationDbContext applicationDbContext, IUserService userService, 
            ILogger<EntityService<TransactionLog>> logger) 
            : base(applicationDbContext, userService, logger)
        {
        }
    }
}
