using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskMeta.Shared.Interfaces;

namespace TaskMeta.Shared.Models.Repositories
{
    public class RepositoryBase<E>
        (ApplicationDbContext applicationDbContext, ICacheProvider cacheProvider, ILogger<E> logger) :
        IRepositoryBase<E> where E : class, IEntity
    {
        protected ApplicationDbContext Context { get; private set; } = applicationDbContext;
        protected ICacheProvider CacheProvider { get; private set; } = cacheProvider;
        protected ILogger Logger { get; private set; } = logger;

        public async Task<List<E>> GetAll()
        {
            try
            {

                var set = Context.Set<E>();
                return await set.ToListAsync<E>();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while getting all entities");
                throw;
            }
        }

        public async Task<List<E>> GetAll(int expiration)
        {
            string key = ListKey();
            var result = CacheProvider.Get<List<E>>(key);

            if (result != null) return result;
            result = await GetAll();
            CacheProvider.Set(key, result, expiration);
            return result;
        }

        public async Task<E?> GetById(int id)
        {
            try
            {
                return await Context.Set<E>().FindAsync(id);
            }
            catch (Exception ex)
            {
                string message = $"An error occurred while getting an entity by id: {id}";
                Logger.LogError(ex, $"{message}");
                throw;
            }
        }
        public async Task<E?> GetById(int id, int expiration)
        {
            string key = Key("ID",id);
            var result = CacheProvider.Get<E>(key);

            result ??= await GetById(id);
            CacheProvider.Set(key, result, expiration);

            return result;
        }

        public void Add(E entity)
        {
            try
            {
                // Add any business logic or validation here
                Context.Set<E>().Add(entity);
                CacheProvider.Clear();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while adding an entity");
                throw;
            }
        }

        public void Update(E entity)
        {
            try
            {
                // Add any business logic or validation here
                Context.Set<E>().Update(entity);
                CacheProvider.Clear();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while updating an entity");
                throw;
            }
        }

        public void Delete(E entity)
        {
            try
            {

                Context.Remove<E>(entity);
                CacheProvider.Clear();

            }
            catch (Exception ex)
            {
                string message = $"An error occurred while deleting an entity";
                Logger.LogError(ex, $"{message}");
                throw;
            }
        }

        protected static string Key() => typeof(E).Name;
        protected static string Key(params object[] parameters) => Key() + string.Join("-", parameters);

        protected static string ListKey() => $"{Key()}-List";
        protected static string ListKey(params object[] parameters) => ListKey() + string.Join("-", parameters);
    }
}
