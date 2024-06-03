using TaskMeta.Components.Pages.ViewModels;

namespace TaskMeta.ViewModels
{
    public static class ViewModels
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services
                .AddScoped<DailyChecklistViewModel>()
                .AddScoped<SummaryViewModel>();

            return services;
        }
    }
}
