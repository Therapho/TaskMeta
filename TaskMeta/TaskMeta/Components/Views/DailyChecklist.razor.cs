using CommunityToolkit.Diagnostics;
using Microsoft.AspNetCore.Components;
using TaskMeta.Shared.Models;

namespace TaskMeta.Components.Views
{
    public partial class DailyChecklist : ComponentBase
    {     
        [Inject]
        private DailyChecklistViewModel? ViewModel { get; set; }


        protected async override Task OnInitializedAsync()
        {
            Guard.IsNotNull(ViewModel);
            if(!ViewModel.Loaded) await ViewModel.Load();

            await base.OnParametersSetAsync();
        }
        private async void HandleTaskChange(TaskActivity task)
        {
            await ViewModel!.UpdateTaskActivity(task);
        }
        private async void HandleJobChange()
        {
            //await ViewModel!.UpdateJob(job);
        }
    }
}
