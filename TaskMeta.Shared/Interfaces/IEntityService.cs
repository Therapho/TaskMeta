
namespace TaskMeta.Shared.Interfaces;
public interface IEntityService<E> where E : class
{

    Task AddAsync(E entity);
    Task DeleteAsync(int id);
    Task<List<E>> GetAllAsync();
    Task<E?> GetByIdAsync(int id);
    Task UpdateAsync(E entity);
}