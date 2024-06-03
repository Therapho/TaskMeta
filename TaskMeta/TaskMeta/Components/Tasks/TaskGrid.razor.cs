using Microsoft.AspNetCore.Components;
using TaskMeta.Data.Services;
using TaskMeta.Shared;
using TaskMeta.Shared.Models;

namespace TaskMeta.Components.Tasks
{
    public partial class TaskGrid : ComponentBase
    {
        private TaskWeek? _taskWeek;
        private List<TaskActivity>? _taskActivityList;

        [Parameter]
        public List<TaskDefinition>? TaskDefinitionList { get; set; }

        [Parameter]
        public TaskWeek? TaskWeek { get => _taskWeek; set => _taskWeek = value; }
        [Parameter]
        public bool CanApprove { get; set; }

        [Parameter]
        public EventCallback OnApproved { get; set; }

        [Parameter]
        public EventCallback<TaskActivity> OnChange { get; set; }

        private bool locked = true;
        protected override void OnParametersSet()
        {
            if (TaskWeek != null)
            {
                _taskWeek = TaskWeek;
                _taskActivityList = [.. _taskWeek.TaskActivityList];
                locked = TaskWeek.StatusId == Constants.Status.Accepted || !CanApprove;
            }

        }

        private async void HandleChange(TaskActivity task)
        {
            // Add informational logging
            Console.WriteLine("Handling task change: " + task.ToString());

            await OnChange.InvokeAsync(task);
        }

        private void HandleApprove(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
        {
            OnApproved.InvokeAsync();
        }
       
    }
}
