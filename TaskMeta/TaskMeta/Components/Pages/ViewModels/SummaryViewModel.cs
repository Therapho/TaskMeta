using Microsoft.AspNetCore.Components;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;
using TaskMeta.Utilities;

namespace TaskMeta.Components.Pages.ViewModels
{
    public class SummaryViewModel(ITaskActivityService taskActivityService , ITaskWeekService taskWeekService,
        ITaskDefinitionService taskDefinitionService, State state, IUserService userService) : ViewModelBase(userService)
    {
        public ITaskActivityService? TaskActivityService { get; private set; } = taskActivityService;
        
        public ITaskWeekService? TaskWeekService { get; private set; } = taskWeekService;
        
        public ITaskDefinitionService? TaskDefinitionService { get; private set; } = taskDefinitionService;
        
        public State? State { get; private set; } = state;

        public List<TaskActivity>? TaskActivityList { get; private set; }
        public List<TaskDefinition>? TaskDefinitionList { get; private set; }
        public List<TaskDefinition>? TaskDefinitionFilteredList { get; private set; }
        public TaskWeek? TaskWeek { get; private set; }
        public DateOnly StartOfWeek { get; private set; }
        public TaskWeek? PreviousWeek { get; private set; }
        public TaskWeek? NextWeek { get; private set; }
        public List<ApplicationUser>? ContributorList { get; private set; }
        public bool CanApprove { get; private set; }

    public override async Task Load()
        {
            await base.Load();
            TaskDefinitionList = await TaskDefinitionService!.GetAllAsync();
            TaskDefinitionFilteredList = TaskDefinitionList;
            if (!IsAdmin)
            {
                await LoadThisWeek(User!);
            }
            else
            {
                ContributorList = await UserService!.GetContributors();
                if (State?.SelectedUser != null)
                {
                    await LoadThisWeek(State.SelectedUser);
                }
            }

        }
        public async Task LoadThisWeek(ApplicationUser user)
        {

            var newTaskWeek = await TaskWeekService!.GetOrCreateCurrentWeek(user!.Id);
            TaskDefinitionFilteredList = TaskDefinitionList?.Where(t => t.UserId == user!.Id).ToList();
            Console.WriteLine("This week loaded/created");
            await LoadActivities(newTaskWeek);
        }

        public async Task AcceptTaskWeek()
        {
            await TaskWeekService!.AcceptWeek(TaskWeek!);

            CanApprove = false;
        }

        public async Task ChangeTask(TaskActivity task)
        {
            if (task.Complete) TaskWeek!.Value += task.Value;
            else TaskWeek!.Value -= task.Value;

            await TaskActivityService!.UpdateAsync(task, false);
            await TaskWeekService!.UpdateAsync(TaskWeek, false);
            await TaskWeekService!.Commit();
        }

        public async Task LoadActivities(TaskWeek newTaskWeek)
        {
            TaskWeek = newTaskWeek;
            TaskActivityList = await TaskActivityService!.GetByTaskWeek(TaskWeek);
            Console.WriteLine($"{TaskActivityList.Count} Activities Loaded");
            (PreviousWeek, NextWeek) = await TaskWeekService!.GetAdjacent(TaskWeek);

            StartOfWeek = TaskWeek.WeekStartDate;
            if (IsAdmin && TaskWeek.StatusId == 1)
            {
                CanApprove = true;
            }
            else
            {
                CanApprove = false;
            }

        }   
    }
}
