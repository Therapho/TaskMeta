using Microsoft.AspNetCore.Components;
using TaskMeta.Shared.Models;


namespace JobMeta.Components.Jobs;

public partial class JobChecklist : ComponentBase
{
    [Parameter]
    public List<Job>? JobList { get; set; }

    [Parameter]
    public EventCallback<Job> OnChange { get; set; }

    public decimal totalValue = 0;

    public bool locked = false;



    private async void HandleChange(Job job)
    {
        await OnChange.InvokeAsync(job);
    }
}
