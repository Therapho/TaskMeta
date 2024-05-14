using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskMeta.Data.Models;

namespace TaskMeta.Data.Repositories;
public class TaskDefinitionRepository : EntityRepository<TaskDefinition>, ITaskDefinitionRepository
{
    public TaskDefinitionRepository(ApplicationDbContext context, ILogger<TaskDefinitionRepository> logger) : base(context, logger) {}

}
