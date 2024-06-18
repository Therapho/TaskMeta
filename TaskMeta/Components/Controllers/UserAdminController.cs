using TaskMeta.Components.ViewModels;
using TaskMeta.MVVM;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Components.Controllers
{
    public class UserAdminController(IUnitOfWork unitOfWork, ApplicationState state, 
        UserEditGridViewModel userEditGridViewModel) : ControllerBase(unitOfWork, state)
    {
        public UserEditGridViewModel UserEditGridViewModel { get; set; } = userEditGridViewModel;
        public override async Task Load()
        {
            await base.Load();
            await UserEditGridViewModel.Load();
            

        }
    }
}
