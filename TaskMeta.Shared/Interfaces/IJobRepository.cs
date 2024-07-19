using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Interfaces;

public interface IJobRepository : IRepositoryBase<Job>
{
    List<Job> GetCurrentJobs();
    List<Job> GetCurrentJobs(ApplicationUser user);
}
