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

    internal async Task Load(TaskWeek taskWeek)
    {
        TaskActivityList = await UnitOfWork.TaskActivityRepository.GetListByTaskWeek(taskWeek);
    }

    public async void HandleChange(TaskActivity taskActivity)
    {
        var taskWeek = taskActivity.TaskWeek;
        Guard.IsNotNull(taskActivity);


        UnitOfWork.TaskWeekRepository.UpdateValue(taskWeek, taskActivity.Value, taskActivity.Complete);
        UnitOfWork.TaskActivityRepository.Update(taskActivity);
        await UnitOfWork.SaveChanges();

        PropertyChanged(nameof(TaskActivity));
    }
}
