using TaskMeta.MVVM;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;
using TaskMeta.Components.ViewModels;

namespace TaskMeta.Components.Controllers
{
    public class DailyChecklistController(IUnitOfWork unitOfWork, ApplicationState state, JobChecklistViewModel jobChecklistViewModel,
        TaskListViewModel taskListViewModel) : ControllerBase(unitOfWork, state)
    {
        public TaskWeek? TaskWeek { get; set; }
        public JobChecklistViewModel JobChecklistViewModel { get; set; } = jobChecklistViewModel;
        public TaskListViewModel TaskListViewModel { get; set; } = taskListViewModel;

        public override async Task Load()
        {
            TaskWeek = await UnitOfWork.GetOrCreateCurrentWeek(State.CurrentUser!);
            await TaskListViewModel.Load(TaskWeek);
            await JobChecklistViewModel.Load(TaskWeek!);
        }
    }
}
