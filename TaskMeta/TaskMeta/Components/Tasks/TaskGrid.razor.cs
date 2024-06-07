using Microsoft.AspNetCore.Components;
using TaskMeta.Data.Repositories;
using TaskMeta.MVVM;
using TaskMeta.Shared;
using TaskMeta.Shared.Models;

namespace TaskMeta.Components.Tasks;

public partial class TaskGrid : ViewBase<TaskGridViewModel>
{
    [Parameter]
    public EventCallback OnChange { get; set; }

    protected override void OnParametersSet()
    {
        ViewModel!.OnChange = OnChange;
        
    }
}
