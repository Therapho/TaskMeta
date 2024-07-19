using TaskMeta.MVVM;
using TaskMeta.Shared;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;
using TaskMeta.Components.ViewModels;

namespace TaskMeta.Components.Controllers
{
    public class SummaryPageController(IUnitOfWork unitOfWork, ApplicationState state,
        JobChecklistViewModel jobChecklistViewModel, TaskGridViewModel taskGridViewModel, UserSelectorViewModel userSelectorViewModel,
        WeekSelectorViewModel weekSelectorViewModel) :
        ControllerBase(unitOfWork, state)
    {
        public JobChecklistViewModel? JobChecklistViewModel { get; set; } = jobChecklistViewModel;
        public TaskGridViewModel? TaskGridViewModel { get; set; } = taskGridViewModel;
        public UserSelectorViewModel UserSelectorViewModel { get; set; } = userSelectorViewModel;
        public WeekSelectorViewModel WeekSelectorViewModel { get; set; } = weekSelectorViewModel;

        public TaskWeek? TaskWeek { get; private set; }
        public ApplicationUser? SelectedUser { get => State?.SelectedUser; set => State!.SelectedUser = value; }


        public bool CanApprove
        {
            get
            {
                return TaskWeek?.StatusId == Constants.Status.Draft && State.IsAdmin;
            }
        }

        public override async Task Load()
        {
            await base.Load();

            if (!State.IsAdmin)
            {
                SelectedUser = State.CurrentUser;
                LoadThisWeek(SelectedUser!);
            }
            else
            {
                await UserSelectorViewModel!.Load();
                if (SelectedUser != null)
                {
                    LoadThisWeek(SelectedUser!);
                }
            }
            
        }
        public void HandleUserSelected(ApplicationUser user)
        {
            SelectedUser = user;
            LoadThisWeek(SelectedUser!);
        }
        private void LoadThisWeek(ApplicationUser user)
        {
            var taskWeek = UnitOfWork.GetOrCreateCurrentWeek(user);
            LoadTaskWeek(taskWeek);

        }
        public void HandleTaskWeekChange(TaskWeek taskWeek)
        {
            LoadTaskWeek(taskWeek);

        }
        private void LoadTaskWeek(TaskWeek taskWeek)
        {
            TaskGridViewModel!.Load(taskWeek);
            JobChecklistViewModel!.Load(taskWeek);
            WeekSelectorViewModel!.Load(taskWeek);
            TaskWeek = taskWeek;
            StateHasChanged!();
        }

        public void HandleApprove()
        {
            UnitOfWork!.AcceptWeek(TaskWeek!);            

            StateHasChanged!();
        }
        public void HandleChange()
        {
            StateHasChanged!();
        }
    }
}
