using CommunityToolkit.Diagnostics;
using TaskMeta.MVVM;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Components.ViewModels;

public class TaskListViewModel(IUnitOfWork unitOfWork, ApplicationState state) : ViewModelBase(unitOfWork, state)
{
    public List<TaskActivity>? TaskActivityList { get; set; }
    public bool Locked { get; set; }
    public Action? OnChange { get; set; }

    internal void Load(TaskWeek taskWeek)
    {
        TaskActivityList = UnitOfWork.GetTaskActivityListByDate(DateTime.Now.ToDateOnly(), taskWeek);
    }

    public void HandleChange(TaskActivity taskActivity)
    {
        var taskWeek = taskActivity.TaskWeek;
        Guard.IsNotNull(taskActivity);


        UnitOfWork.UpdateTaskWeekValue(taskWeek, taskActivity.Value, taskActivity.Complete);
        UnitOfWork.UpdateTaskActivity(taskActivity);

        OnChange?.Invoke();
    }
}
