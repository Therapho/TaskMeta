
namespace TaskMeta.Data.Repositories
{
    public interface IEntityRepository<E> where E : class
    {
        Task AddAsync(E entity);
        Task DeleteAsync(int id);
        Task<List<E>> GetAllAsync();
        Task<E?> GetByIdAsync(int id);
        Task UpdateAsync(E entity);
    }
}