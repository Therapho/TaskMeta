namespace TaskMeta.Components.Controllers
{
    public static class PageControllerExtensions
    {
        public static IServiceCollection AddPageControllers(this IServiceCollection services)
        {
            services
                .AddScoped<DailyChecklistController>()
                .AddScoped<SummaryPageController>()
                .AddScoped<JobAdminController>()
                .AddScoped<FundAdminController>()
                .AddScoped<TransactionController>()
                .AddScoped<TaskAdminController>()
                .AddScoped<UserAdminController>();
            return services;
        }
    }
}