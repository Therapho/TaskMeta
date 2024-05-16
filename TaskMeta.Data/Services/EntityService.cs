using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskMeta.Shared.Interfaces;

namespace TaskMeta.Data.Services
{
    public class EntityService<E> : IEntityService<E> where E : class
    {
        protected ApplicationDbContext Context { get; private set; }
        protected IUserService UserService { get; private set; }
        protected ILogger<EntityService<E>> Logger { get; private set; }

        public EntityService(ApplicationDbContext applicationDbContext, IUserService userService, ILogger<EntityService<E>> logger)
        {
            Context = applicationDbContext;
            UserService = userService;
            Logger = logger;
        }

        public async Task<List<E>> GetAllAsync()
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

        public async Task<E?> GetByIdAsync(int id)
        {
            try
            {
                return await Context.Set<E>().FindAsync(id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"An error occurred while getting an entity by id: {id}");
                throw;
            }
        }

        public async Task AddAsync(E entity)
        {
            try
            {
                // Add any business logic or validation here
                await Context.Set<E>().AddAsync(entity);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while adding an entity");
                throw;
            }
        }

        public async Task UpdateAsync(E entity)
        {
            try
            {
                // Add any business logic or validation here
                Context.Set<E>().Update(entity);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while updating an entity");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                // Add any business logic or validation here
                var entity = await Context.FindAsync<E>(id);
                if (entity != null)
                {
                    Context.Remove<E>(entity);
                    await Context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"An error occurred while deleting an entity with id: {id}");
                throw;
            }
        }
    }
}
