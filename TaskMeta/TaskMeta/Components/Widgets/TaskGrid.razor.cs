using Microsoft.AspNetCore.Components;
using TaskMeta.Shared.Models;

namespace TaskMeta.Components.Widgets
{
    public partial class TaskGrid : ComponentBase
    {
        [Parameter]
        public List<TaskDefinition>? TaskDefinitions { get; set; }

        [Parameter]
        public List<TaskActivity>? TaskActivities { get; set; }

        [Parameter]
        public TaskWeek? TaskWeek { get; set; }


    }
}
