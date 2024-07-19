using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.MVVM;

public class ViewModelBase(IUnitOfWork unitOfWork, ApplicationState state)
{
    public bool Loaded { get; set; }
    public IUnitOfWork UnitOfWork { get; private set; } = unitOfWork;
    public ApplicationState State { get; set; } = state;
    public Action? StateHasChanged { get; set; }
   

}
