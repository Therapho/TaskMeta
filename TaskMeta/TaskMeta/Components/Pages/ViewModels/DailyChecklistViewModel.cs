using CommunityToolkit.Diagnostics;
using TaskMeta.Data.Services;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.ViewModels;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Components.Pages.ViewModels
{
    public class DailyChecklistViewModel(IJobService jobService, ITaskActivityService taskActivityService,
        ITaskWeekService taskWeekService, IUserService userService) : ViewModelBase(userService)
    {
        private readonly IJobService _jobService = jobService;
        private readonly ITaskActivityService _taskActivityService = taskActivityService;
        private readonly ITaskWeekService _taskWeekService = taskWeekService;

        private TaskWeek? _taskWeek;

        public List<Job>? JobList { get; private set; }
        public List<TaskActivity>? TaskActivityList { get; private set; }
        public bool IsLocked { get; set; }
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Loads the jobs for the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        public override  async Task Load()
        {
            await base.Load();

            _taskWeek = await _taskWeekService.GetOrCreateCurrentWeek(User!.Id);

            Guard.IsNotNull(_taskActivityService);
            Guard.IsNotNull(_jobService);
            Guard.IsNotNull(_taskWeek);

            TotalValue = _taskWeek.Value;
            JobList = await _jobService.GetCurrentJobs(User);
            TaskActivityList = await _taskActivityService.GetOrCreateTaskActivities(_taskWeek, DateTime.Now.ToDateOnly());
            
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

            await _jobService.UpdateAsync(job, false);
            await _taskWeekService.UpdateAsync(_taskWeek, false);
            await _taskWeekService.Commit();
        }

        public async Task UpdateTaskActivity(TaskActivity taskActivity)
        {
            if (taskActivity.Complete) _taskWeek!.Value += taskActivity.Value;
            else _taskWeek!.Value -= taskActivity.Value;

            TotalValue = _taskWeek.Value;

            await _taskWeekService.UpdateAsync(_taskWeek, false);
            await _taskActivityService.UpdateAsync(taskActivity, false);
            await _taskWeekService.Commit();
        }

    }
}
