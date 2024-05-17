using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.IdentityModel.Tokens;
using TaskMeta.Data.Services;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

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

    protected override async Task OnInitializedAsync()
    {
        var user = await UserService!.GetCurrentUser();

        startOfWeek = Tools.StartOfWeek;
        taskWeek = await TaskWeekService!.GetOrCreateCurrentWeek(user.Id);
        taskActivities = await TaskActivityService!.GetByTaskWeek(taskWeek);
        taskDefinitions = await TaskDefinitionService!.GetAllAsync();
        await base.OnInitializedAsync();
    }
}
