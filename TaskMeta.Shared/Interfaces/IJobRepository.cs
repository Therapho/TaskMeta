using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Interfaces;

public interface IJobRepository : IRepositoryBase<Job>
{
    Task<List<Job>> GetCurrentJobs();
    Task<List<Job>> GetCurrentJobs(ApplicationUser user);
}
