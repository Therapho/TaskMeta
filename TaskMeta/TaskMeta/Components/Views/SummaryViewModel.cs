using CommunityToolkit.Diagnostics;
using Microsoft.AspNetCore.Components;
using TaskMeta.Components.Jobs;
using TaskMeta.Components.Tasks;
using TaskMeta.MVVM;
using TaskMeta.Shared;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;
using TaskMeta.ViewModels;

namespace TaskMeta.Components.Views
{
    public class SummaryViewModel : ViewModelBase
    {
        public JobChecklistViewModel? JobChecklistViewModel { get; set; }
        public TaskGridViewModel? TaskGridViewModel { get; set; }

        public State? State { get; private set; }

        public TaskWeek? TaskWeek { get; private set; }
        public TaskWeek? PreviousWeek { get; private set; }
        public TaskWeek? NextWeek { get; private set; }
        public List<ApplicationUser>? ContributorList { get; private set; }
        public decimal TotalValue { get; set; }
        public ApplicationUser? SelectedUser { get => State?.SelectedUser; set => State!.SelectedUser = value; }

         
        public bool CanApprove
        {
            get
            {
                return TaskWeek?.StatusId == Constants.Status.Draft && IsAdmin;
            }
        }

        public SummaryViewModel(IUnitOfWork unitOfWork, State state,
            JobChecklistViewModel jobCheckListViewModel, TaskGridViewModel taskGridViewModel) : base(unitOfWork)
        {
            Guard.IsNotNull(jobCheckListViewModel);
            Guard.IsNotNull(taskGridViewModel);
            Guard.IsNotNull(state);

            jobCheckListViewModel.OnChange += HandleChange;
            JobChecklistViewModel = jobCheckListViewModel;
            taskGridViewModel.OnChange += HandleChange;
            TaskGridViewModel = taskGridViewModel;
            State = state;
        }

        public override async Task Load()
        {
            await base.Load();

            if (!IsAdmin)
            {
                SelectedUser = User;
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

        }
        public async Task HandleUserSelected(ApplicationUser user)
        {
            SelectedUser = user;
            await LoadThisWeek(SelectedUser!);
        }
        private async Task LoadThisWeek(ApplicationUser user)
        {
            TaskWeek = await UnitOfWork.GetOrCreateCurrentWeek(user);
            await UnitOfWork.SaveChanges();
            await TaskGridViewModel!.Load(TaskWeek);
            await JobChecklistViewModel!.Load(TaskWeek);


        }
        public void HandleChange()
        {
            TotalValue = TaskWeek!.Value;
            StateHasChanged!();
        }

        public void HandleNextWeekClicked()
        {
            TaskWeek = NextWeek;
        }

        public void HandlePreviousWeekClicked()
        {
            TaskWeek = PreviousWeek;
        }

        public async void HandleApprove()
        {
            await UnitOfWork!.AcceptWeek(TaskWeek!);
            await UnitOfWork!.SaveChanges();

            StateHasChanged!();
        }
    }
}
