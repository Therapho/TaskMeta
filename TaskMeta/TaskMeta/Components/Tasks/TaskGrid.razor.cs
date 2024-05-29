using Microsoft.AspNetCore.Components;
using TaskMeta.Data.Services;
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
        public EventCallback<TaskActivity> OnChange { get; set; }

        private bool locked = true;
        protected override void OnParametersSet()
        {
            if (TaskWeek != null)
            {
                taskWeek = TaskWeek;
                taskActivities = [.. TaskWeek.TaskActivities];
                locked = TaskWeek.StatusId == Constants.Status.Accepted || !CanApprove;
            }

        }

        private async void HandleChange(TaskActivity task) => await OnChange.InvokeAsync(task);
        private void HandleApprove(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
        {
            OnApproved.InvokeAsync();
        }
        private void HandleCheckChange(TaskActivity task)
        {

            task.Complete = !task.Complete;
            HandleChange(task);

        }
        private void HandleCheckChange(int sequence, DateOnly date)
        {
            var definition = TaskDefinitions!.Where(d => d.Sequence == sequence).First();
            TaskActivity task = new ()
            {
                Complete = true,
                Description = definition.Description,
                Sequence = definition.Sequence,
                TaskDate = date,
                TaskDefinitionId = definition.Id,
                TaskWeekId = taskWeek!.Id,
                Value = definition.Value

            };
            HandleChange(task);
        }
    }
}
