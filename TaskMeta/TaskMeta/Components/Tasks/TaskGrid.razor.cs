using Microsoft.AspNetCore.Components;
using TaskMeta.Shared;
using TaskMeta.Shared.Models;

namespace TaskMeta.Components.Tasks
{
    public partial class TaskGrid : ComponentBase
    {
        private TaskWeek? taskWeek;
        private List<TaskActivity>? taskActivities;

        [Parameter]
        public List<TaskDefinition>? TaskDefinitions { get; set; }

        [Parameter]
        public TaskWeek? TaskWeek { get => taskWeek; set => taskWeek = value; }
        [Parameter]
        public bool CanApprove { get; set; }

        [Parameter]
        public EventCallback OnApproved { get; set; }

        [Parameter]
        public bool CanEdit { get; set; }

        [Parameter]
        public EventCallback<TaskActivity> OnChange { get; set; }

        private bool locked = true;
        protected override void OnParametersSet()
        {
            if (TaskWeek != null)
            {
                taskWeek = TaskWeek;
                taskActivities = [.. TaskWeek.TaskActivities];
                locked = TaskWeek.StatusId == Constants.Status.Accepted || !CanEdit;
            }
            
        }

        private async void HandleChange(TaskActivity task) => await OnChange.InvokeAsync(task);
        private void HandleApprove(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
        {
            OnApproved.InvokeAsync();
        }
    }
}
