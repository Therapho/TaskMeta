using TaskMeta.Components.Controllers;

namespace TaskMeta.Components.ViewModels;

public static class ViewModelExtensions
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services           
            .AddScoped<TaskGridViewModel>()
            .AddScoped<JobChecklistViewModel>()            
            .AddScoped<JobEditGridViewModel>()
            .AddScoped<UserSelectorViewModel>()
            .AddScoped<WeekSelectorViewModel>()
            .AddScoped<TaskListViewModel>()            
            .AddScoped<TransactionListViewModel>()
            .AddScoped<TransactionFormViewModel>();

        return services;
    }
}
