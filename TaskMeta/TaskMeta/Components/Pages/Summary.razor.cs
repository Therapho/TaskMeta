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
    private bool canApprove = false;
    protected override async Task OnInitializedAsync()
    {
        taskDefinitions = await TaskDefinitionService!.GetAllAsync();

        var user = await UserService!.GetCurrentUser();
        if(user == null) throw new InvalidOperationException("User is null");
        isAdmin = await UserService!.IsAdmin(user);
        if (!isAdmin)
        {
            
            await LoadThisWeek(user);
        }
        else
        {
            contributors = await UserService!.GetContributors();

        }
        
        await base.OnInitializedAsync();
    }
    async Task LoadThisWeek(ApplicationUser user)
    {
        selectedUser = user;
        var newTaskWeek = await TaskWeekService!.GetOrCreateCurrentWeek(user!.Id);
        Console.WriteLine("This week loaded/created");
        await LoadActivitiesk(newTaskWeek);
        
    }
    async void HandleUserSelected(ApplicationUser user)
    {
        Console.WriteLine("User select handled.");
        await LoadThisWeek(user);

    }
    async void HandleApproval()
    {
        if (taskWeek != null)
        {
            taskWeek.StatusId = 2;
            await TaskWeekService!.UpdateAsync(taskWeek);
            canApprove = false;
        }
    }
    async Task LoadActivitiesk(TaskWeek newTaskWeek)
    {
        taskWeek = newTaskWeek;
        taskActivities = await TaskActivityService!.GetByTaskWeek(taskWeek);
        Console.WriteLine($"{taskActivities.Count} Activities Loaded");
        //taskActivities = newTaskWeek.TaskActivities.ToList();
        (previousWeek, nextWeek) = await TaskWeekService!.GetAdjacent(taskWeek);
        
        startOfWeek = taskWeek.WeekStartDate;
        if(isAdmin && taskWeek.StatusId == 1)
        {
            canApprove = true;
        }
        else
        {
            canApprove = false;
        }
        StateHasChanged();
    }
}
