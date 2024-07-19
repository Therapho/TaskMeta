using Microsoft.Extensions.DependencyInjection;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models.Repositories;
using Microsoft.FluentUI.AspNetCore.Components;
using TaskMeta.Shared.Models.Providers;
using Microsoft.Extensions.Caching.Memory;

namespace TaskMeta.Shared.Models
{
    public static class DataAccessLayer
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services)
        {
            services
                .AddScoped<ITaskActivityRepository, TaskActivityRepository>()
                .AddScoped<ITaskDefinitionRepository, TaskDefinitionRepository>()
                .AddScoped<ITaskWeekRepository, TaskWeekRepository>()
                .AddScoped<IFundRepository, FundRepository>()
                .AddScoped<ITransactionLogRepository, TransactionLogRepository>()
                .AddScoped<IJobRepository, JobRepository>()
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<ICacheProvider, CacheProvider>()
                .AddScoped<IMemoryCache, MemoryCache>();
            return services;
            
        }

    }
}
