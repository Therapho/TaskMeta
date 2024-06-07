using CommunityToolkit.Diagnostics;
using TaskMeta.MVVM;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Components.Views
{
    public class DailyChecklistViewModel(IUnitOfWork unitOfWork) : ViewModelBase(unitOfWork)
    {
        private TaskWeek? _taskWeek;

        public List<Job>? JobList { get; private set; }
        public List<TaskActivity>? TaskActivityList { get; private set; }
        public bool IsLocked { get; set; }
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Loads the jobs for the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        public async Task Load(TaskWeek taskWeek)
        {
            Guard.IsNotNull(taskWeek);

            await base.Load();

            _taskWeek = taskWeek;

            TotalValue = _taskWeek.Value;
            JobList = await UnitOfWork.JobRepository.GetCurrentJobs(User!);
            TaskActivityList = await UnitOfWork.TaskActivityRepository.GetListByDate(DateTime.Now.ToDateOnly(), User!);

        }

        /// <summary>
        /// Updates the specified job.
        /// </summary>
        /// <param name="job">The job to update.</param>
        public async Task UpdateJob(Job job)
        {
            if (job.Complete)
            {
                _taskWeek!.Value += job.Value;
                job.DateCompleted = DateTime.Now.ToDateOnly();
            }
            else
            {
                _taskWeek!.Value -= job.Value;
                job.DateCompleted = null;
            }

            TotalValue = _taskWeek.Value;

            UnitOfWork.JobRepository.Update(job);
            UnitOfWork.TaskWeekRepository.Update(_taskWeek);
            await UnitOfWork.SaveChanges();
        }

        public async Task UpdateTaskActivity(TaskActivity taskActivity)
        {
            if (taskActivity.Complete) _taskWeek!.Value += taskActivity.Value;
            else _taskWeek!.Value -= taskActivity.Value;

            TotalValue = _taskWeek.Value;

            UnitOfWork.TaskWeekRepository.Update(_taskWeek);
            UnitOfWork.TaskActivityRepository.Update(taskActivity);
            await UnitOfWork.SaveChanges();
        }

    }
}
