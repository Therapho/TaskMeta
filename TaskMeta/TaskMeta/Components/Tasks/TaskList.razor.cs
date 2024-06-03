using CommunityToolkit.Diagnostics;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using TaskMeta.Shared;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Components.Tasks;

public partial class TaskList : ComponentBase
{
    [Parameter]
    public List<TaskActivity>? TaskActivities { get; set; }

    [Parameter]
    public EventCallback<TaskActivity> OnChange { get; set; }

    [Parameter]
    public bool Locked  { get; set; }

    private async void HandleChange(TaskActivity task)
    {
        Guard.IsNotNull(task);
        
        await OnChange.InvokeAsync(task);
    }
}
