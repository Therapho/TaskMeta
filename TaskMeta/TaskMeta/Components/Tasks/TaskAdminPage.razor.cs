using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;
using TaskMeta.Components.Transactions;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;


namespace TaskMeta.Components.Tasks
{
    public partial class TaskAdminPage : ComponentBase, IDisposable
    {
        [Inject]
        public IUserService? UserService { get; set; }

        [Inject]
        public State? State { get; set; }

        [Inject]
        public ITaskDefinitionService? TaskDefinitionService { get; set; }

        private List<ApplicationUser>? _contributors;

        private List<TaskDefinition>? _taskDefinitionList;
        private List<TaskDefinition>? _taskDefinitionListFiltered;

        private TaskDefinition? _editTask;
        private EditContext? editContext = new(new TaskDefinition());
        private ValidationMessageStore? messageStore;

        protected override async Task OnInitializedAsync()
        {
            _taskDefinitionList = await TaskDefinitionService!.GetTaskDefinitionList();
            _contributors = await UserService!.GetContributors();
            if (State?.SelectedUser != null && _taskDefinitionList != null)
            {
                _taskDefinitionListFiltered = _taskDefinitionList!.Where(t => t.UserId == State.SelectedUser.Id).ToList();
            }
            else
            {
                _taskDefinitionListFiltered = _taskDefinitionList;
            }

            await base.OnInitializedAsync();
        }
        private void SetupForm(TaskDefinition taskDefinition)
        {
            editContext = new(taskDefinition);

            editContext.OnValidationRequested += HandleValidationRequested;

            messageStore = new(editContext);
        }

        private void HandleValidationRequested(object? sender, ValidationRequestedEventArgs e)
        {
            messageStore?.Clear();

            if (String.IsNullOrEmpty(_editTask?.Description))
            {
                messageStore?.Add(() => _editTask!.Description, "Description is required.");
            }
            if( _editTask?.Value == null)
            {
                messageStore?.Add(() => _editTask!.Description, "Value is required.");
            }
            if (_editTask?.Sequence == null)
            {
                messageStore?.Add(() => _editTask!.Description, "Sequence is required.");
            }
        }

        void HandleEdit(TaskDefinition taskDefinition)
        {
            _editTask = taskDefinition;
            SetupForm(taskDefinition);
            StateHasChanged();
        }
        void HandleUserSelected(ApplicationUser user)
        {
            State!.SelectedUser = user;
            if(user != null) _taskDefinitionListFiltered = _taskDefinitionList!.Where(t => t.UserId == user.Id).ToList();
            else _taskDefinitionListFiltered = _taskDefinitionList;
            StateHasChanged();
        }
        async void HandleDelete(TaskDefinition taskDefinition)
        {
            await TaskDefinitionService!.DeleteAsync(taskDefinition.Id);
            _taskDefinitionListFiltered!.Remove(taskDefinition);
            StateHasChanged();
        }
        void HandleAdd()
        {
            _editTask = new TaskDefinition();
            _taskDefinitionList!.Add(_editTask);
            SetupForm(_editTask);
            StateHasChanged();
        }
        async void HandleSave()
        {
            if(_editTask?.Id == 0)
            {
                await TaskDefinitionService!.AddAsync(_editTask, false);
            }
            await TaskDefinitionService!.Commit();
            _editTask = null;
            StateHasChanged();

        }
        void HandleCancel()
        {
            if (_editTask != null && _editTask.Id == 0) _taskDefinitionList!.Remove(_editTask);
            _editTask = null;
            StateHasChanged();
        }
        void HandleUpdateActive(TaskDefinition task, bool value)
        {

            task.Active = value;
            TaskDefinitionService!.Commit();
            
        }
        void TearDownForm()
        {
            if (editContext != null)
            {
                editContext.OnValidationRequested -= HandleValidationRequested;
            }
        }

        public void Dispose()
        {
            TearDownForm();
        }
    }
}
