using TaskMeta.Data.Models;
namespace TaskMeta.Data.Repositories;
public interface ITaskActivityRepository:IEntityRepository<TaskActivity>
{
 
    Task AddAsync(List<TaskActivity> taskActivityList);
    Task<List<TaskActivity>> GetByDate(DateOnly date);
 }