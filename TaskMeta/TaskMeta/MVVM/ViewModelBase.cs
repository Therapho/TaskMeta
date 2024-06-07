using CommunityToolkit.Diagnostics;
using TaskMeta.Data.Repositories;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Utilities;

namespace TaskMeta.MVVM
{
    public class ViewModelBase(IUnitOfWork unitOfWork)
    {
        public bool Loaded { get; set; }
        public IUnitOfWork UnitOfWork { get; private set; } = unitOfWork;
        public ApplicationUser? User { get; set; }
        public bool IsAdmin { get; set; }
        public Action? StateHasChanged { get; set; }

        public virtual async Task Load()
        {
            User = await UnitOfWork.UserRepository.GetCurrentUser();
            Guard.IsNotNull(User);
            IsAdmin = await UnitOfWork.UserRepository.IsAdmin();

            Loaded = true;
        }
    }
}
