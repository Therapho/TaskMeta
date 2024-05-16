﻿using Microsoft.AspNetCore.Components.Authorization;
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

        protected ApplicationDbContext Context { get; set; }
        protected ILogger<EntityRepository<E>> Logger { get; set; }

        public EntityRepository(ApplicationDbContext context, ILogger<EntityRepository<E>> logger)
        {
            Context = context;
            Logger = logger;
        }

        public async Task<List<E>> GetAllAsync()
        {

            try
            {
                return await Context.Set<E>().ToListAsync<E>();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while getting Es");
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
                Logger.LogError(ex, $"An error occurred while getting E with id {id}");
                throw;
            }
        }

        public async Task AddAsync(E entity)
        {
            try
            {
                await Context.AddAsync<E>(entity);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while adding a entity");
                throw;
            }
        }

        public async Task UpdateAsync(E entity)
        {
            try
            {
                Context.Update<E>(entity);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"An error occurred while updating entity  {entity}");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await Context.FindAsync<E>(id);
                if (entity != null)
                {
                    Context.Remove<E>(entity);
                    await Context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"An error occurred while deleting E with id {id}");
                throw;
            }
        }
    }

}
