
namespace TaskMeta.Shared.Interfaces;
public interface IEntityService<E> where E : class
{

    Task AddAsync(E entity, bool commit = true);
    Task DeleteAsync(int id, bool commit = true);
    Task<List<E>> GetAllAsync();
    Task<E?> GetByIdAsync(int id);
    Task UpdateAsync(E entity, bool commit = true);

    Task Commit();
}