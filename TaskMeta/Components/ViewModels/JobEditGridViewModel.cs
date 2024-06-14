using CommunityToolkit.Diagnostics;
using Microsoft.AspNetCore.Components.Forms;
using TaskMeta.MVVM;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Components.ViewModels;

public class JobEditGridViewModel(IUnitOfWork unitOfWork, ApplicationState state) : ViewModelBase(unitOfWork, state), IDisposable
{
    public List<ApplicationUser>? UserList { get; set; }
    public List<Job>? JobListFiltered { get; set; }
    public Job? EditJob { get; set; }
    public EditContext? EditContext { get; set; } = new(new Job());

    private ValidationMessageStore? messageStore;
    private List<Job>? _jobList;
    private List<ApplicationUser>? _contributorList;
    private ApplicationUser? _selectedUser;

    public async Task Load(ApplicationUser selectedUser)
    {
        Guard.IsNotNull(selectedUser);

        _jobList = await UnitOfWork!.JobRepository!.GetCurrentJobs();
        _contributorList = await UnitOfWork!.UserRepository!.GetContributors();
        UserList = [.. _contributorList];
        UserList.Insert(0, new ApplicationUser() { UserName = "Assign to user...." });

        if (selectedUser != null && _jobList != null)
        {
            JobListFiltered = _jobList!.Where(t => t.UserId == selectedUser.Id).ToList();
        }
        else
        {
            JobListFiltered = _jobList;
        }
        _selectedUser = selectedUser!;
        StateHasChanged!();
    }

    private void SetupForm(Job Job)
    {
        EditContext = new(Job);

        EditContext.OnValidationRequested += HandleValidationRequested;

        messageStore = new(EditContext);
    }

    private void HandleValidationRequested(object? sender, ValidationRequestedEventArgs e)
    {
        messageStore?.Clear();

        if (String.IsNullOrEmpty(EditJob?.Description))
        {
            messageStore?.Add(() => EditJob!.Description, "Description is required.");
        }
        if (EditJob?.Value == null)
        {
            messageStore?.Add(() => EditJob!.Description, "Value is required.");
        }
        if (EditJob?.DateDue == null)
        {
            messageStore?.Add(() => EditJob!.Description, "Sequence is required.");
        }
        if (String.IsNullOrEmpty(EditJob?.UserId))
        {
            messageStore?.Add(() => EditJob!.Description, "User is required.");
        }
    }

    public void HandleEdit(Job Job)
    {
        EditJob = Job;
        SetupForm(Job);
        StateHasChanged!();
    }

    public async void HandleDelete(Job Job)
    {
        UnitOfWork!.JobRepository!.Delete(Job);
        await UnitOfWork!.SaveChanges();
        _jobList!.Remove(Job);
        JobListFiltered!.Remove(Job);
        StateHasChanged!();
    }

    public void HandleAdd()
    {
        EditJob = new Job()
        {
            DateAssigned = DateTime.Now.ToDateOnly(),
            DateDue = DateTime.Now.ToDateOnly(),
            UserId = _selectedUser?.Id
        };
        _jobList!.Add(EditJob);
        JobListFiltered!.Add(EditJob);
        SetupForm(EditJob);
        StateHasChanged!();
    }
    public async void HandleSave()
    {
        if (EditJob?.Id == 0)
        {
            UnitOfWork!.JobRepository!.Add(EditJob);
            await UnitOfWork!.SaveChanges();
        }
        EditJob = null;
        StateHasChanged!();

    }
    public void HandleCancel()
    {
        if (EditJob != null && EditJob.Id == 0) _jobList!.Remove(EditJob);
        EditJob = null;
        StateHasChanged!();
    }

    private void TearDownForm()
    {
        if (EditContext != null)
        {
            EditContext.OnValidationRequested -= HandleValidationRequested;
        }
    }

    public void Dispose()
    {
        TearDownForm();
        GC.SuppressFinalize(this);
    }
}

