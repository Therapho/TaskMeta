using TaskMeta.MVVM;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Components.ViewModels;

public class JobChecklistViewModel(IUnitOfWork unitOfWork, ApplicationState state) : ViewModelBase(unitOfWork, state)
{
    public TaskWeek? TaskWeek { get; private set; }

    public List<Job>? JobList { get; private set; }
    public bool IsLocked { get; private set; }

    public Action? OnChange { get; set; }

    /// <summary>
    /// Loads the jobs for the specified user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    public void Load(TaskWeek taskWeek)
    {
        if (taskWeek == null) return;
        TaskWeek = taskWeek;
        JobList = UnitOfWork!.GetCurrentJobs(TaskWeek!.User);
        StateHasChanged!();
    }

    /// <summary>
    /// Updates the specified job.
    /// </summary>
    /// <param name="job">The job that has changed.</param>
    public void HandleChange(Job job)
    {
        job.DateCompleted = job.Complete ? DateTime.Now.ToDateOnly() : null;
        UnitOfWork!.UpdateJob(job);
        
        OnChange?.Invoke();
    }
}
