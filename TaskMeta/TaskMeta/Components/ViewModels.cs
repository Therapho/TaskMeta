using TaskMeta.Components.Jobs;
using TaskMeta.Components.Views;
using TaskMeta.Components.Tasks;

namespace TaskMeta.ViewModels
{
    public static class ViewModels
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services
                .AddScoped<DailyChecklistViewModel>()
                .AddScoped<SummaryViewModel>()
                .AddScoped<TaskGridViewModel>()
                .AddScoped<JobChecklistViewModel>();


            return services;
        }
    }
}
