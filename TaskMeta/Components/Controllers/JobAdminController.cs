using CommunityToolkit.Diagnostics;
using TaskMeta.MVVM;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;
using TaskMeta.Components.ViewModels;

namespace TaskMeta.Components.Controllers;

public class JobAdminController(IUnitOfWork unitOfWork, JobEditGridViewModel jobEditGridViewModel, UserSelectorViewModel userSelectorViewModel,
    ApplicationState state) : ControllerBase(unitOfWork, state)
{
    public JobEditGridViewModel? JobEditGridViewModel { get; private set; } = jobEditGridViewModel;
    public UserSelectorViewModel? UserSelectorViewModel { get; private set; } = userSelectorViewModel;

    public async override Task Load()
    {
        await base.Load();

        UserSelectorViewModel!.Load();

        if (State!.SelectedUser != null)
        {
            JobEditGridViewModel!.Load(State!.SelectedUser);
        }
        
    }
    public void HandleUserSelected(ApplicationUser user)
    {
        Guard.IsNotNull(user);
        JobEditGridViewModel!.Load(user!);
    }
}
