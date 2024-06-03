using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Interfaces;

public interface IJobService : IEntityService<Job>
{
    Task<List<Job>> GetCurrentJobs();
    Task<List<Job>> GetCurrentJobs(ApplicationUser user);
}
