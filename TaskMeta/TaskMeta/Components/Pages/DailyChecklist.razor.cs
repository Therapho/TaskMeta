using CommunityToolkit.Diagnostics;
using Microsoft.AspNetCore.Components;
using TaskMeta.Components.Pages.ViewModels;
using TaskMeta.Shared.Models;

namespace TaskMeta.Components.Pages
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
        private async Task HandleTaskChange(TaskActivity task)
        {
            await ViewModel!.UpdateTaskActivity(task);
        }
        private async Task HandleJobChange(Job job)
        {
            await ViewModel!.UpdateJob(job);
        }
    }
}
