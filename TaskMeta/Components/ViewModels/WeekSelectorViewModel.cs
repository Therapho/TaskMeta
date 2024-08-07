﻿using CommunityToolkit.Diagnostics;
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

    public void Load(TaskWeek? currentWeek)
    {
        Guard.IsNotNull(currentWeek);
        SetCurrentWeek(currentWeek);

    }
    private void SetCurrentWeek(TaskWeek? taskWeek)
    {
        CurrentWeek = taskWeek;
        (PreviousWeek, NextWeek) = UnitOfWork.GetAdjacentTaskWeeks(CurrentWeek!);
        StateHasChanged!();
    }
    public void HandlePreviousWeekClicked() => OnChange!(PreviousWeek!);
    public void HandleNextWeekClicked() => OnChange!(NextWeek!);


}
