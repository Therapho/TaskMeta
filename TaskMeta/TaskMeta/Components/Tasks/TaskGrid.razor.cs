using Microsoft.AspNetCore.Components;
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

        protected override void OnParametersSet()
        {
            if (TaskWeek != null)
            {
                taskWeek = TaskWeek;
                taskActivities = TaskWeek.TaskActivities.ToList();

            }
        }
        private void HandleApprove(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
        {
            OnApproved.InvokeAsync();
        }
    }
}
