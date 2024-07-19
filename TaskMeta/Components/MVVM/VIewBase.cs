using Microsoft.AspNetCore.Components;

namespace TaskMeta.MVVM;

public class ViewBase<VM> : ComponentBase where VM : ViewModelBase
{
    [Parameter, EditorRequired]
    public VM? ViewModel { get; set; }


    protected override void OnInitialized()
    {
        ViewModel!.StateHasChanged = StateHasChanged;
        base.OnInitialized();

    }

}
