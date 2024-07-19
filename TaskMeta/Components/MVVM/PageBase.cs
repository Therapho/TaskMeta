using Microsoft.AspNetCore.Components;

namespace TaskMeta.MVVM;

public class PageBase<C> : ComponentBase where C : ControllerBase
{
    [Inject]
    public C? Controller { get; set; }
    protected override void OnInitialized()
    {
        
        base.OnInitialized();
    }
    protected override async Task OnParametersSetAsync()
    {
        Controller!.StateHasChanged = StateHasChanged;
        await Controller!.Load();
        await base.OnParametersSetAsync();
    }
}
