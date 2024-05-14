using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMeta.Data.Models;

namespace TaskMeta.Data.Repositories
{
    public class EntityRepository<E> : IEntityRepository<E> where E : class
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EntityRepository<E>> _logger;

        public EntityRepository(ApplicationDbContext context, ILogger<EntityRepository<E>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<E>> GetAllAsync()
        {

            try
            {
                return await _context.Set<E>().ToListAsync<E>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting Es");
                throw;
            }
        }

        public async Task<E?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<E>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting E with id {id}");
                throw;
            }
        }

        public async Task AddAsync(E entity)
        {
            try
            {
                await _context.AddAsync<E>(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a entity");
                throw;
            }
        }

        public async Task UpdateAsync(E entity)
        {
            try
            {
                _context.Update<E>(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating entity  {entity}");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.FindAsync<E>(id);
                if (entity != null)
                {
                    _context.Remove<E>(entity);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting E with id {id}");
                throw;
            }
        }
    }

}

