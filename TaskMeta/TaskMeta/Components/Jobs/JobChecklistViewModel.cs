using TaskMeta.MVVM;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Components.Jobs;

public class JobChecklistViewModel(IUnitOfWork unitOfWork) : ViewModelBase(unitOfWork)
{


    public event Action? OnChange;
    // generate an event OnChange

    public TaskWeek? TaskWeek { get; private set; }

    public List<Job>? JobList { get; private set; }
    public bool IsLocked { get; private set; }


    /// <summary>
    /// Loads the jobs for the specified user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    public async Task Load(TaskWeek taskWeek)
    {
        if (taskWeek == null) return;
        TaskWeek = taskWeek;
        await base.Load();

        JobList = await UnitOfWork!.JobRepository.GetCurrentJobs(TaskWeek!.User);
        StateHasChanged!();
    }

    /// <summary>
    /// Updates the specified job.
    /// </summary>
    /// <param name="job">The job that has changed.</param>
    public async void HandleChange(Job job)
    {
        job.DateCompleted = job.Complete ? DateTime.Now.ToDateOnly() : null;

        UnitOfWork!.JobRepository.Update(job);
        UnitOfWork!.TaskWeekRepository.UpdateValue(TaskWeek!, job.Value, job.Complete);
        await UnitOfWork!.SaveChanges();
        OnChange?.Invoke();
    }
}
