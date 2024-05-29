using Microsoft.Extensions.DependencyInjection;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Data.Services;
using Microsoft.FluentUI.AspNetCore.Components;

namespace TaskMeta.Data
{
    public static class DataAccessLayer
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services)
        {
            services
                .AddScoped<ITaskActivityService, TaskActivityService>()
                .AddScoped<ITaskDefinitionService, TaskDefinitionService>()
                .AddScoped<ITaskWeekService, TaskWeekService>()
                .AddScoped<IFundService, FundService>()
                .AddScoped<ITransactionLogService, TransactionLogService>();
            return services;
            
        }

    }
}
