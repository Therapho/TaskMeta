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
        public List<ApplicationUser>? ContributorList { get; private set; }
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
                await LoadThisWeek(SelectedUser!);
            }
            else
            {
                ContributorList = await UnitOfWork!.UserRepository!.GetContributors();
                if (SelectedUser != null)
                {
                    await LoadThisWeek(SelectedUser!);
                }
            }
            WeekSelectorViewModel!.OnChange += HandleTaskWeekChange;
            await UserSelectorViewModel!.Load();
        }
        public async void HandleUserSelected(ApplicationUser user)
        {
            SelectedUser = user;
            await LoadThisWeek(SelectedUser!);
        }
        private async Task LoadThisWeek(ApplicationUser user)
        {
            var taskWeek = await UnitOfWork.GetOrCreateCurrentWeek(user);
            await UnitOfWork.SaveChanges();
            await LoadTaskWeek(taskWeek);

        }
        public async void HandleTaskWeekChange(TaskWeek taskWeek)
        {
            await LoadTaskWeek(taskWeek);

        }
        private async Task LoadTaskWeek(TaskWeek taskWeek)
        {
            await TaskGridViewModel!.Load(taskWeek);
            await JobChecklistViewModel!.Load(taskWeek);
            await WeekSelectorViewModel!.Load(taskWeek);
            TaskWeek = taskWeek;
            StateHasChanged!();
        }

        public async void HandleApprove()
        {
            await UnitOfWork!.AcceptWeek(TaskWeek!);
            await UnitOfWork!.SaveChanges();

            StateHasChanged!();
        }
        public void HandleChange()
        {
            StateHasChanged!();
        }
    }
}
