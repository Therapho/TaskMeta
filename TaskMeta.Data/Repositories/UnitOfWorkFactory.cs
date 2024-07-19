using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskMeta.Data.Repositories;
using TaskMeta.Shared.Interfaces;

namespace TaskMeta.Data.Repositories;

public class UnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    private readonly ILogger<IUnitOfWork> _logger;
    private readonly IUserRepository _userRepository;

    public UnitOfWorkFactory(IDbContextFactory<ApplicationDbContext> contextFactory, ILogger<IUnitOfWork> logger,
        IUserRepository userRepository)
    {
        _contextFactory = contextFactory;
        _logger = logger;
        _userRepository = userRepository;
    }

    public IUnitOfWork Create()
    {
        var context = _contextFactory.CreateDbContext();
        var taskDefinitionRepository = new TaskDefinitionRepository(context, _logger);
        var jobRepository = new JobRepository(context, _logger);
        var taskActivityRepository = new TaskActivityRepository(context, _logger);
        var taskWeekRepository = new TaskWeekRepository(context, _logger);
        var fundRepository = new FundRepository(context, _logger);
        var transactionLogRepository = new TransactionLogRepository(context, _logger);

        return new UnitOfWork(context, taskDefinitionRepository, jobRepository, taskActivityRepository, taskWeekRepository,
            fundRepository, transactionLogRepository, _userRepository, _logger);
    }
}
