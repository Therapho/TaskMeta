using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.IdentityModel.Tokens;
using TaskMeta.Data.Services;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace TaskMeta.Components.Pages;

public partial class Summary : ComponentBase
{
    [Inject]
    public ITaskActivityService? TaskActivityService { get; set; }
    [Inject]
    public ITaskWeekService? TaskWeekService { get; set; }
    [Inject]
    public ITaskDefinitionService? TaskDefinitionService { get; set; }

    [Inject]
    public IUserService? UserService { get; set; }

    private List<TaskActivity>? taskActivities;
    private List<TaskDefinition>? taskDefinitions;
    private TaskWeek? taskWeek;
    private DateOnly startOfWeek;
    private TaskWeek? previousWeek;
    private TaskWeek? nextWeek;
    private ApplicationUser? selectedUser;
    private bool isAdmin;
    private List<ApplicationUser>? contributors;
    protected override async Task OnInitializedAsync()
    {
        taskDefinitions = await TaskDefinitionService!.GetAllAsync();

        var user = await UserService!.GetCurrentUser();
        if(user == null) throw new InvalidOperationException("User is null");
        isAdmin = await UserService!.IsAdmin(user);
        if (!isAdmin)
        {
            
            await LoadWeek(user);
        }
        else
        {
            contributors = await UserService!.GetContributors();
        }
        
        await base.OnInitializedAsync();
    }
    async Task LoadWeek(ApplicationUser user)
    {
        taskWeek = await TaskWeekService!.GetOrCreateCurrentWeek(user!.Id);
        await LoadWeek(taskWeek);
        selectedUser = user;
    }
    async void HandleUserSelected(ApplicationUser user)
    {
        await LoadWeek(user);
    }
    async Task LoadWeek(TaskWeek newTaskWeek)
    {
        taskActivities = await TaskActivityService!.GetByTaskWeek(newTaskWeek);
        //taskActivities = newTaskWeek.TaskActivities.ToList();
        (previousWeek, nextWeek) = await TaskWeekService!.GetAdjacent(newTaskWeek);
        taskWeek = newTaskWeek;
        startOfWeek = taskWeek.WeekStartDate;
    }
}
