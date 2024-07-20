using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.MVVM;

public class ControllerBase(IUnitOfWork unitOfWork, ApplicationState state) : ViewModelBase(unitOfWork, state)
{
    private readonly List<ViewModelBase> list = [];


    public virtual async Task Load()
    {
        State.CurrentUser = await UnitOfWork.GetCurrentUser();
        State.IsAdmin = await UnitOfWork.IsAdmin();
        Loaded = true;
    }

}
