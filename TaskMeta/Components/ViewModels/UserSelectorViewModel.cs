using TaskMeta.MVVM;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Components.ViewModels;

public class UserSelectorViewModel(IUnitOfWork unitOfWork, ApplicationState state) : ViewModelBase(unitOfWork, state)
{
    public List<ApplicationUser>? ContributorList { get; private set; }

    public Action<ApplicationUser>? OnChange { get; set; }
    public bool CanClear { get; set; }
    public ApplicationUser? SelectedUser { get => State?.SelectedUser; }

    public async Task Load()
    {
        ContributorList = await UnitOfWork!.UserRepository!.GetContributors();
        //StateHasChanged!();
    }

    public void HandleUserSelected(ApplicationUser user)
    {
        State.SelectedUser = user;

        OnChange?.Invoke(user);
    }
}
