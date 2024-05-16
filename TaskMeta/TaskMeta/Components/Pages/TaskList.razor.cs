using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;

namespace TaskMeta.Components.Pages
{
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

        protected override async Task OnInitializedAsync()
        {
            var user = await UserService.GetCurrentUser();
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            taskWeek = await TaskWeekService.GetOrCreateCurrentWeek(user.Id);
            taskActivities = await TaskActivityService.GetOrCreateTaskActivities(taskWeek);
        }

        private async void HandleChange(TaskActivity task)
        {
            if(task.Complete)
            {
                taskWeek.Value += task.Value;
            }
            else
            {
                taskWeek.Value -= task.Value;
            }
            await TaskActivityService.UpdateAsync(task);
            await TaskWeekService.UpdateAsync(taskWeek);
        }
    }
}
