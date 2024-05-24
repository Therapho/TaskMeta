using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TaskMeta.Shared.Models;

namespace TaskMeta.Components.Widgets
{
    public partial class UserSelector : ComponentBase
    {
        [Parameter]
        public List<ApplicationUser>? Users { get; set; }

        [Parameter]
        public ApplicationUser? SelectedUser { get; set; }

        [Parameter]
        public EventCallback<ApplicationUser> OnSelect { get; set; }

        private async Task HandleButtonClick(ApplicationUser user)
        {
            SelectedUser = user;
            await OnSelect.InvokeAsync(user);
            
        }
        
    }
}
