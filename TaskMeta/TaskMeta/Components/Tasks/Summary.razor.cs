using Microsoft.AspNetCore.Components;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Components.Tasks;

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

    [Inject]
    public State? State { get; set; }

    private List<TaskActivity>? taskActivities;
    private List<TaskDefinition>? taskDefinitions;
    private TaskWeek? taskWeek;
    private DateOnly startOfWeek;
    private TaskWeek? previousWeek;
    private TaskWeek? nextWeek;
    private bool isAdmin;
    private List<ApplicationUser>? contributors;
    private bool canApprove = false;
    protected override async Task OnInitializedAsync()
    {
        taskDefinitions = await TaskDefinitionService!.GetAllAsync();

        var user = await UserService!.GetCurrentUser();
        if (user == null) throw new InvalidOperationException("User is null");
        isAdmin = await UserService!.IsAdmin(user);
        if (!isAdmin)
        {
            await LoadThisWeek(user);        
        }
        else
        {
            contributors = await UserService!.GetContributors();
            if (State?.SelectedUser != null)
            {
                await LoadThisWeek(State.SelectedUser);
            }
        }

        await base.OnInitializedAsync();
    }
    async Task LoadThisWeek(ApplicationUser user)
    {
        State!.SelectedUser = user;
        var newTaskWeek = await TaskWeekService!.GetOrCreateCurrentWeek(user!.Id);
        Console.WriteLine("This week loaded/created");
        await LoadActivities(newTaskWeek);
    }
    async void HandleUserSelected(ApplicationUser user)
    {
        await LoadThisWeek(user);
    }
    async void HandleApproval()
    {
        if (taskWeek != null)
        {
            await TaskWeekService!.AcceptWeek(taskWeek);

            canApprove = false;
            StateHasChanged();
        }
    }
    async Task LoadActivities(TaskWeek newTaskWeek)
    {
        taskWeek = newTaskWeek;
        taskActivities = await TaskActivityService!.GetByTaskWeek(taskWeek);
        Console.WriteLine($"{taskActivities.Count} Activities Loaded");
        //taskActivities = newTaskWeek.TaskActivities.ToList();
        (previousWeek, nextWeek) = await TaskWeekService!.GetAdjacent(taskWeek);

        startOfWeek = taskWeek.WeekStartDate;
        if (isAdmin && taskWeek.StatusId == 1)
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
