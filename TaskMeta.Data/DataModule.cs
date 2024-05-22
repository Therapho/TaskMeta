using Microsoft.Extensions.DependencyInjection;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Data.Services;

namespace TaskMeta.Data
{
    public static class DataModule
    {
        public static IServiceCollection AddDataModule(this IServiceCollection services)
        {
            services             
                .AddScoped<ITaskActivityService, TaskActivityService>()
                .AddScoped<ITaskDefinitionService, TaskDefinitionService>()
                .AddScoped<ITaskWeekService, TaskWeekService>()
                .AddScoped<IFundService, FundService>();
            return services;
        }
    }
}
