

namespace TaskMeta.Shared.Interfaces;
public interface IRepositoryBase<E> where E : class
{

    void AddJob(E entity);
    void Delete(E entity);
    List<E> GetAll();
    List<E> GetAll(int expiration);
    E? GetById(int id);
    void Update(E entity);

}