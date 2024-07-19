using Microsoft.FluentUI.AspNetCore.Components;
using TaskMeta.MVVM;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Components.ViewModels
{
    public class TransactionListViewModel (IUnitOfWork unitOfWork, ApplicationState state)
        : ViewModelBase( unitOfWork,  state)
    {
        public List<TransactionLog>? TransactionList { get; set; }

        public PaginationState Pagination = new() { ItemsPerPage = 10 };

        public void Load(ApplicationUser user)
        {
            TransactionList = UnitOfWork!.TransactionLogRepository!.GetTransactionsByUser(user.Id, 1, 20);
            StateHasChanged!();
        }
    }
}
