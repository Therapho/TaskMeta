using Microsoft.AspNetCore.Components;

namespace TaskMeta.MVVM
{
    public class ComponentBase<VM> : ComponentBase where VM : ViewModelBase
    {
        [Parameter]
        public VM? ViewModel { get; set; }

        protected override void OnInitialized()
        {
            ViewModel!.StateHasChanged = StateHasChanged;
        }
    }
   
}
