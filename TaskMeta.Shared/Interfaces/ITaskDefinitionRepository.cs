using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Interfaces;
public interface ITaskDefinitionRepository : IRepositoryBase<TaskDefinition>
{
    Task<List<TaskDefinition>> GetList();
    Task<List<TaskDefinition>> GetListByUser(ApplicationUser user);
}