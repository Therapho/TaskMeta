using CommunityToolkit.Diagnostics;
using TaskMeta.MVVM;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Components.ViewModels;

public class WeekSelectorViewModel(IUnitOfWork unitOfWork, ApplicationState state) : ViewModelBase(unitOfWork, state)
{
    public TaskWeek? CurrentWeek { get; set; }
    public TaskWeek? NextWeek { get; set; }
    public TaskWeek? PreviousWeek { get; set; }
    public Action<TaskWeek>? OnChange { get; set; }



    public async Task Load(TaskWeek? currentWeek)
    {
        Guard.IsNotNull(currentWeek);
        await SetCurrentWeek(currentWeek);

    }
    private async Task SetCurrentWeek(TaskWeek? taskWeek)
    {
        CurrentWeek = taskWeek;
        (PreviousWeek, NextWeek) = await UnitOfWork.TaskWeekRepository.GetAdjacent(CurrentWeek!);
        StateHasChanged!();
    }
    public async Task HandlePreviousWeekClicked()
    {
        OnChange!(PreviousWeek!);
        await SetCurrentWeek(PreviousWeek);

    }
    public async Task HandleNextWeekClicked()
    {
        OnChange!(NextWeek!);
        await SetCurrentWeek(NextWeek);

    }


}
