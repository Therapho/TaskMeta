using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Xml.Linq;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Components.Jobs;

public partial class JobAdminPage : ComponentBase, IDisposable
{
    [Inject]
    public State? State { get; set; }

    [Inject]
    public IUnitOfWork? UnitOfWork { get; set; }

    private List<ApplicationUser>? _contributorList;
    private List<ApplicationUser>? _userList;
    private List<Job>? _jobList;
    private List<Job>? _jobListFiltered;

    private Job? _editJob;
    private EditContext? _editContext = new(new Job());
    private ValidationMessageStore? messageStore;

    protected override async Task OnInitializedAsync()
    {
        _jobList = await UnitOfWork!.JobRepository!.GetCurrentJobs();
        _contributorList = await UnitOfWork!.UserRepository!.GetContributors();
        _userList = [.. _contributorList];
        _userList.Insert(0, new ApplicationUser() { UserName = "Assign to user...." });
        if (State?.SelectedUser != null && _jobList != null)
        {
            _jobListFiltered = _jobList!.Where(t => t.UserId == State.SelectedUser.Id).ToList();
        }
        else
        {
            _jobListFiltered = _jobList;
        }

        await base.OnInitializedAsync();
    }
    private void SetupForm(Job Job)
    {
        _editContext = new(Job);

        _editContext.OnValidationRequested += HandleValidationRequested;

        messageStore = new(_editContext);
    }

    private void HandleValidationRequested(object? sender, ValidationRequestedEventArgs e)
    {
        messageStore?.Clear();

        if (String.IsNullOrEmpty(_editJob?.Description))
        {
            messageStore?.Add(() => _editJob!.Description, "Description is required.");
        }
        if (_editJob?.Value == null)
        {
            messageStore?.Add(() => _editJob!.Description, "Value is required.");
        }
        if (_editJob?.DateDue == null)
        {
            messageStore?.Add(() => _editJob!.Description, "Sequence is required.");
        }
        if(String.IsNullOrEmpty(_editJob?.UserId))
        {
            messageStore?.Add(() => _editJob!.Description, "User is required.");
        }
    }

    void HandleEdit(Job Job)
    {
        _editJob = Job;
        SetupForm(Job);
        StateHasChanged();
    }
    void HandleUserSelected(ApplicationUser user)
    {
        State!.SelectedUser = user;
        if (user != null) _jobListFiltered = _jobList!.Where(t => t.UserId == user.Id).ToList();
        else _jobListFiltered = _jobList;
        StateHasChanged();
    }
    //async void HandleDelete(Job Job)
    //{
    //    UnitOfWork!.JobRepository!.Delete(Job);
    //    await UnitOfWork!.SaveChanges();
    //    _jobListFiltered!.Remove(Job);
    //    StateHasChanged();
    //}

    void HandleAdd()
    {
        _editJob = new Job()
        {
            DateAssigned = DateTime.Now.ToDateOnly(),
            DateDue = DateTime.Now.ToDateOnly()
        };
        _jobList!.Add(_editJob);
        SetupForm(_editJob);
        StateHasChanged();
    }
    async void HandleSave()
    {
        if (_editJob?.Id == 0)
        {
            UnitOfWork!.JobRepository!.Add(_editJob);
            await UnitOfWork!.SaveChanges();
        }
        _editJob = null;
        StateHasChanged();

    }
    void HandleCancel()
    {
        if (_editJob != null && _editJob.Id == 0) _jobList!.Remove(_editJob);
        _editJob = null;
        StateHasChanged();
    }

    void TearDownForm()
    {
        if (_editContext != null)
        {
            _editContext.OnValidationRequested -= HandleValidationRequested;
        }
    }

    public void Dispose()
    {
        TearDownForm();
        GC.SuppressFinalize(this);
    }
}

