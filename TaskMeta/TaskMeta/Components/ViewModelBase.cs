using CommunityToolkit.Diagnostics;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Utilities;

namespace TaskMeta.Components
{
    public class ViewModelBase(IUserService userService)
    {
        public bool Loaded { get; set; }
        public IUserService UserService { get; private set; } = userService;
        public ApplicationUser? User { get; set; }
        public bool IsAdmin { get; set; }

        public virtual async Task Load()
        {
            Guard.IsNotNull(UserService);
            

            User = await UserService.GetCurrentUser();
            Guard.IsNotNull(User);
            IsAdmin = await UserService.IsAdmin();

            Loaded = true;
        }
    }
}
