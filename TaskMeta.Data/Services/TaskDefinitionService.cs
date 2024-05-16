using Microsoft.Extensions.Logging;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;

namespace TaskMeta.Data.Services;
public class TaskDefinitionService : EntityService<TaskDefinition>, ITaskDefinitionService
{

    public TaskDefinitionService(ApplicationDbContext applicationDbContext, IUserService userService, ILogger<EntityService<TaskDefinition>> logger) 
        :base(applicationDbContext, userService, logger) {}

}
