namespace TaskMeta.Components.Controllers
{
    public static class PageControllerExtensions
    {
        public static IServiceCollection AddPageControllers(this IServiceCollection services)
        {
            services
                .AddScoped<DailyChecklistPageController>()
                .AddScoped<SummaryPageController>()
                .AddScoped<JobAdminPageController>()
                .AddScoped<FundListController>()
                .AddScoped<TransactionController>();

            return services;
        }
    }
}