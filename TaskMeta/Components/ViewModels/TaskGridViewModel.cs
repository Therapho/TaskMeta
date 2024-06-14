using CommunityToolkit.Diagnostics;
using TaskMeta.MVVM;
using TaskMeta.Shared;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Components.ViewModels;

public class TaskGridViewModel(IUnitOfWork unitOfWork, ApplicationState state) : ViewModelBase(unitOfWork, state)
{
    public TaskWeek? TaskWeek { get; set; }
    public List<TaskActivity>? TaskActivityList { get; private set; }
    public bool Locked { get; private set; }
    public List<TaskDefinition>? TaskDefinitionList { get; private set; }
    public Action? OnChange { get; set; }

    public void Load(TaskWeek taskWeek)
    {
        Guard.IsNotNull(taskWeek);
        TaskWeek = taskWeek;

        TaskDefinitionList = UnitOfWork.TaskDefinitionRepository.GetListByUser(TaskWeek.User);
        TaskActivityList = UnitOfWork.TaskActivityRepository.GetListByTaskWeek(TaskWeek);
        Locked = TaskWeek.StatusId == Constants.Status.Accepted || !State.IsAdmin;
        StateHasChanged!();
    }
    public void HandleChange(TaskActivity task)
    {
        UnitOfWork!.TaskActivityRepository!.Update(task);
        UnitOfWork!.UpdateTaskWeekValue(TaskWeek!, task.Value, task.Complete);
        OnChange?.Invoke();
    }

}
