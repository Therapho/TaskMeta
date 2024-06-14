using Microsoft.AspNetCore.Components.Forms;
using TaskMeta.MVVM;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;
using TaskMeta.Components.ViewModels;

namespace TaskMeta.Components.Controllers
{
    public class TaskAdminController(IUnitOfWork unitOfWork, ApplicationState state, UserSelectorViewModel userSelectorViewModel) : ControllerBase(unitOfWork, state)
    {

        public List<ApplicationUser>? Contributors { get; set; }

        public List<TaskDefinition>? TaskDefinitionList { get; set; }
        public List<TaskDefinition>? TaskDefinitionListFiltered { get; set; }

        public TaskDefinition? EditTask { get; set; }
        public EditContext? EditContext { get; set; } = new(new TaskDefinition());
        private ValidationMessageStore? messageStore;

        public UserSelectorViewModel UserSelectorViewModel { get; set; } = userSelectorViewModel;
        public async override Task Load()
        {
            await base.Load();
            TaskDefinitionList = await UnitOfWork!.TaskDefinitionRepository!.GetList();
            Contributors = await UnitOfWork!.UserRepository!.GetContributors();
            UpdateFilter();

        }

        private void UpdateFilter()
        {
            if (State?.SelectedUser != null && TaskDefinitionList != null)
            {
                TaskDefinitionListFiltered = TaskDefinitionList!.Where(t => t.UserId == State.SelectedUser.Id).ToList();
            }
            else
            {
                TaskDefinitionListFiltered = TaskDefinitionList;
            }
        }

        private void SetupForm(TaskDefinition taskDefinition)
        {
            EditContext = new(taskDefinition);

            EditContext.OnValidationRequested += HandleValidationRequested;

            messageStore = new(EditContext);
        }

        private void HandleValidationRequested(object? sender, ValidationRequestedEventArgs e)
        {
            messageStore?.Clear();

            if (string.IsNullOrEmpty(EditTask?.Description))
            {
                messageStore?.Add(() => EditTask!.Description, "Description is required.");
            }
            if (EditTask?.Value == null)
            {
                messageStore?.Add(() => EditTask!.Description, "Value is required.");
            }
            if (EditTask?.Sequence == null)
            {
                messageStore?.Add(() => EditTask!.Description, "Sequence is required.");
            }
        }

        public void HandleEdit(TaskDefinition taskDefinition)
        {
            EditTask = taskDefinition;
            SetupForm(taskDefinition);
            StateHasChanged!();
        }

        private void HandleUserSelected(ApplicationUser user)
        {
            State!.SelectedUser = user;
            if (user != null) TaskDefinitionListFiltered = TaskDefinitionList!.Where(t => t.UserId == user.Id).ToList();
            else TaskDefinitionListFiltered = TaskDefinitionList;
            StateHasChanged!();
        }

        public void HandleAdd()
        {
            EditTask = new TaskDefinition();
            EditTask.UserId = State?.SelectedUser?.Id;
            TaskDefinitionList!.Add(EditTask);
            UpdateFilter();
            SetupForm(EditTask);
            StateHasChanged!();
        }

        public async void HandleSave()
        {
            if (EditTask?.Id == 0)
            {
                UnitOfWork!.TaskDefinitionRepository!.Add(EditTask);
                await UnitOfWork!.SaveChanges();
            }

            EditTask = null;
            StateHasChanged!();

        }

        public void HandleCancel()
        {
            if (EditTask != null && EditTask.Id == 0)
            {
                TaskDefinitionList!.Remove(EditTask);
                UpdateFilter();
            }
            EditTask = null;
            StateHasChanged!();
        }

        public async void HandleUpdateActive(TaskDefinition task, bool value)
        {

            task.Active = value;
            UnitOfWork!.TaskDefinitionRepository!.Update(task);
            await UnitOfWork!.SaveChanges();

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
}

