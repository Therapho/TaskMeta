

namespace TaskMeta.Shared.Interfaces;
public interface IRepositoryBase<E> where E : class
{

    void Add(E entity);
    void Delete(E entity);
    Task<List<E>> GetAll();
    Task<List<E>> GetAll(int expiration);
    Task<E?> GetById(int id);
    void Update(E entity);

}