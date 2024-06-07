using CommunityToolkit.Diagnostics;
using Microsoft.AspNetCore.Components;
using TaskMeta.MVVM;
using TaskMeta.Shared;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Components.Tasks
{
    public class TaskGridViewModel (IUnitOfWork unitOfWork): ViewModelBase(unitOfWork)
    {
        public TaskWeek? TaskWeek { get; set; }
        public List<TaskActivity>? TaskActivityList { get; private set; }
        public bool Locked { get; private set; }
        public List<TaskDefinition>? TaskDefinitionList { get; private set; }

        public event Action? OnChange;

        public async Task Load(TaskWeek taskWeek)
        {
            Guard.IsNotNull(taskWeek);
            TaskWeek = taskWeek;

            await base.Load();

            TaskDefinitionList = await UnitOfWork.TaskDefinitionRepository.GetListByUser(TaskWeek.User);
            TaskActivityList = await UnitOfWork.TaskActivityRepository.GetListByTaskWeek(TaskWeek);
            Locked = TaskWeek.StatusId == Constants.Status.Accepted || !IsAdmin;          
            StateHasChanged!();
        }
        public async void HandleChange(TaskActivity task)
        {
            UnitOfWork!.TaskActivityRepository!.Update(task);
            UnitOfWork!.TaskWeekRepository!.UpdateValue(TaskWeek!, task.Value, task.Complete);
            await UnitOfWork!.SaveChanges();
            OnChange?.Invoke();
        }

    }
}
