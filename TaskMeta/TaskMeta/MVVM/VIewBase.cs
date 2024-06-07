using Microsoft.AspNetCore.Components;

namespace TaskMeta.MVVM
{
    public class ViewBase<VM> : ComponentBase where VM : ViewModelBase 
    {
        [Inject]
        public VM? ViewModel { get; set; }

        protected override void OnInitialized()
        {
            ViewModel!.StateHasChanged = StateHasChanged;
        }
    }
}
