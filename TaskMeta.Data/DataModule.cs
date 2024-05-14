using Microsoft.Extensions.DependencyInjection;
using TaskMeta.Data.Repositories;
using TaskMeta.Data.Services;

namespace TaskMeta.Data
{
    public static class DataModule
    {
        public static void Build(IServiceCollection services)
        {
            services
                .AddScoped<ITaskActivityRepository, TaskActivityRepository>()
                .AddScoped<ITaskActivityService, TaskActivityService>()
                .AddScoped<ITaskDefinitionRepository, TaskDefinitionRepository>()
                .AddScoped<ITaskDefinitionService, TaskDefinitionService>();
        }
    }
}
