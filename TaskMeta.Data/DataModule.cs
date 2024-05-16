using Microsoft.Extensions.DependencyInjection;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Data.Services;

namespace TaskMeta.Data
{
    public static class DataModule
    {
        public static void Build(IServiceCollection services)
        {
            services             
                .AddScoped<ITaskActivityService, TaskActivityService>()
                .AddScoped<ITaskDefinitionService, TaskDefinitionService>()
                .AddScoped<ITaskWeekService, TaskWeekService>();
        }
    }
}
