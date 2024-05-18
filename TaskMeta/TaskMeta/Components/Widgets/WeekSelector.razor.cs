using Microsoft.AspNetCore.Components;
using TaskMeta.Shared.Models;

namespace TaskMeta.Components.Widgets
{
    public partial class WeekSelector :ComponentBase
    {
        [Parameter]
        public TaskWeek? CurrentWeek { get; set; }
        [Parameter]
        public TaskWeek? NextWeek { get; set; }
        [Parameter]
        public TaskWeek? PreviousWeek { get; set; }

        [Parameter]
        public EventCallback OnPreviousWeek { get; set; }
        [Parameter]
        public EventCallback OnNextWeek { get; set; }

        public async Task HandleNextWeek()
        {
            await OnNextWeek.InvokeAsync();
        }
        public async Task HandlePreviousWeek()
        {
            await OnPreviousWeek.InvokeAsync();
        }
    }
}
