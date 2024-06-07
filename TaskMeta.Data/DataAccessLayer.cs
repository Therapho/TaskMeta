using Microsoft.Extensions.DependencyInjection;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Data.Repositories;
using Microsoft.FluentUI.AspNetCore.Components;

namespace TaskMeta.Data
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
                .AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
            
        }

    }
}
