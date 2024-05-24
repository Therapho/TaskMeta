using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using TaskMeta.Shared;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;

namespace TaskMeta.Components.Tasks;

public partial class TaskList : ComponentBase
{
    List<TaskActivity>? taskActivities;
    private TaskWeek? taskWeek;

    [Inject]
    private IUserService? UserService { get; set; }
    [Inject] 
    private ITaskWeekService? TaskWeekService { get; set; }
    [Inject] 
    private ITaskActivityService? TaskActivityService { get; set; }

    public decimal totalValue = 0;

    public bool locked = false;

    protected override async Task OnInitializedAsync()
    {
        Debug.Assert(TaskWeekService != null, "TaskWeekService != null");
        Debug.Assert(TaskActivityService != null, "TaskActivityService != null");
        Debug.Assert(UserService != null, "UserService != null");

        var user = await UserService.GetCurrentUser();
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        taskWeek = await TaskWeekService.GetOrCreateCurrentWeek(user.Id);
        taskActivities = await TaskActivityService.GetOrCreateTaskActivities(taskWeek);

        totalValue = taskActivities.Where(t=>t.Complete).Sum(t => t.Value);
        locked = taskWeek.StatusId == Constants.Status.Accepted;
    }

    private async void HandleChange(TaskActivity task)
    {
        if (taskWeek == null) throw new InvalidOperationException("TaskWeek is null");


        if (task.Complete)
        {
            taskWeek.Value += task.Value;
            totalValue += task.Value;
        }
        else
        {
            taskWeek.Value -= task.Value;
            totalValue -= task.Value;
        }
        await TaskActivityService!.UpdateAsync(task);
        await TaskWeekService!.UpdateAsync(taskWeek);

    }
}
