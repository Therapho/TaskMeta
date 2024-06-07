using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskMeta.Shared.Interfaces;

namespace TaskMeta.Data.Repositories
{
    public class RepositoryBase<R>
        (ApplicationDbContext applicationDbContext, ILogger<R> logger) :
        IRepositoryBase<R> where R : class
    {
        protected ApplicationDbContext Context { get; private set; } = applicationDbContext;
        protected ILogger Logger { get; private set; } = logger;

        public async Task<List<R>> GetAll()
        {
            try
            {
                var set = Context.Set<R>();
                return await set.ToListAsync<R>();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while getting all entities");
                throw;
            }
        }

        public async Task<R?> GetById(int id)
        {
            try
            {
                return await Context.Set<R>().FindAsync(id);
            }
            catch (Exception ex)
            {
                string message = $"An error occurred while getting an entity by id: {id}";
                Logger.LogError(ex, $"{message}");
                throw;
            }
        }

        public void Add(R entity)
        {
            try
            {
                // Add any business logic or validation here
                Context.Set<R>().Add(entity);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while adding an entity");
                throw;
            }
        }

        public void Update(R entity)
        {
            try
            {
                // Add any business logic or validation here
                Context.Set<R>().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while updating an entity");
                throw;
            }
        }

        public void Delete(R entity)
        {
            try
            {

                Context.Remove<R>(entity);

            }
            catch (Exception ex)
            {
                string message = $"An error occurred while deleting an entity";
                Logger.LogError(ex, $"{message}");
                throw;
            }
        }
    }
}
