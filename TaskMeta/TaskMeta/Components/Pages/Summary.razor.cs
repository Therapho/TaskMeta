using CommunityToolkit.Diagnostics;
using Microsoft.AspNetCore.Components;
using TaskMeta.Components.Pages.ViewModels;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Components.Pages;

public partial class Summary : ComponentBase
{
    [Inject]
    private SummaryViewModel? ViewModel { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Guard.IsNotNull(ViewModel);
        if (!ViewModel.Loaded) await ViewModel.Load();
        await base.OnInitializedAsync();
    }

    async void HandleUserSelected(ApplicationUser user)
    {
        ViewModel!.State!.SelectedUser = user;
        await ViewModel!.LoadThisWeek(user);

        StateHasChanged();
    }
    async void HandleApproval()
    {        
        await ViewModel!.AcceptTaskWeek();
        StateHasChanged();

    }
    async void HandleTaskChange(TaskActivity task)
    {
        await ViewModel!.ChangeTask(task);

        StateHasChanged();
    }
    async void HandleNextWeekClicked()
    {
        await ViewModel!.LoadActivities(ViewModel!.NextWeek!);
        StateHasChanged();
    }
    async void HandlePreviousWeekClicked()
    {
        await ViewModel!.LoadActivities(ViewModel!.PreviousWeek!);
        StateHasChanged();
    }
}
