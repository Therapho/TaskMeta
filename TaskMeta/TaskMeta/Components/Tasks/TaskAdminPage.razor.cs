using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;


namespace TaskMeta.Components.Tasks
{
    public partial class TaskAdminPage : ComponentBase
    {
        [Inject]
        public IUserService? UserService { get; set; }

        [Inject]
        public State? State { get; set; }

        [Inject]
        public ITaskDefinitionService? TaskDefinitionService { get; set; }

        private List<ApplicationUser>? _contributors;

        private IQueryable<TaskDefinition>? _taskDefinitionQuery;
        private IQueryable<TaskDefinition>? _taskDefinitionQueryFiltered;

        PaginationState _pagination = new PaginationState { ItemsPerPage = 10 };

        protected override async Task OnInitializedAsync()
        {
            _taskDefinitionQuery = TaskDefinitionService!.GetTaskDefinitionsQuery();
            _contributors = await UserService!.GetContributors();
            if(State?.SelectedUser != null)
            {
                //_taskDefinitionQueryFiltered = _taskDefinitionQuery!.Where(t => t.UserId == State.SelectedUser.Id);
            }
            else
            {
                //_taskDefinitionQueryFiltered = _taskDefinitionQuery;
            }
            await base.OnInitializedAsync();
        }

        void HandleEdit(TaskDefinition taskDefinition)
        {
        }
        void HandleUserSelected(ApplicationUser user)
        {
            State!.SelectedUser = user;
            _taskDefinitionQueryFiltered = _taskDefinitionQuery!.Where(t => t.UserId == user.Id);
            StateHasChanged();
        }
    }
}
