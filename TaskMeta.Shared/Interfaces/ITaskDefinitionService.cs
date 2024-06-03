using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Interfaces;
public interface ITaskDefinitionService : IEntityService<TaskDefinition>
{
    Task<List<TaskDefinition>> GetList();
    Task<List<TaskDefinition>> GetListByUser(string userId);
}