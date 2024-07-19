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

    public override async Task Load()
    {

        await UserSelectorViewModel!.Load();

        if (State!.SelectedUser != null)
        {
            await JobEditGridViewModel!.Load(State!.SelectedUser);
        }
        await base.Load();
    }
    public async void HandleUserSelected(ApplicationUser user)
    {
        Guard.IsNotNull(user);
        await JobEditGridViewModel!.Load(user!);
    }
}
