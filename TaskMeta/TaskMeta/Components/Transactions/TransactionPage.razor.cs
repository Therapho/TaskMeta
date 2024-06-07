using Microsoft.AspNetCore.Components;
using TaskMeta.Data.Repositories;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Components.Transactions
{

    public partial class TransactionPage :ComponentBase
    {      

        [Inject]
        public State? State { get; set; }

        [Inject]
        public IUnitOfWork? UnitOfWork { get; set; }

        private bool isAdmin;
        private List<ApplicationUser>? contributorList;
        private IQueryable<TransactionLog>? transactionQuery;


        EditMode editMode = EditMode.None;
        private ApplicationUser? loggedInUser;
        private List<Fund>? _fundList;

        protected override async Task OnInitializedAsync()
        {
            
            loggedInUser = await UnitOfWork!.UserRepository!.GetCurrentUser();
            if (loggedInUser == null) throw new InvalidOperationException("User is null");
            isAdmin = await UnitOfWork!.UserRepository!.IsAdmin(loggedInUser);
            if (!isAdmin)
            {
               LoadTransactions(loggedInUser);
            }
            else
            {
                contributorList = await UnitOfWork!.UserRepository!.GetContributors();
                if (State?.SelectedUser != null)
                {
                    LoadTransactions(State.SelectedUser);
                    _fundList = await UnitOfWork!.FundRepository!.GetFundsByUser(State!.SelectedUser!.Id);
                }
            }

            
            await base.OnInitializedAsync();
        }

        private void LoadTransactions(ApplicationUser user)
        {
            transactionQuery = UnitOfWork!.TransactionLogRepository!.QueryTransactionsByUser(user.Id);
        }

        void HandleUserSelected(ApplicationUser user)
        {
            State!.SelectedUser = user;
            LoadTransactions(user);
        }
        void HandleDeposit()
        {
            editMode = EditMode.Deposit;
            StateHasChanged();
        }
        void HandleWithdraw()
        {
            editMode = EditMode.Withdraw;
            StateHasChanged();
        }
        void HandleTransfer()
        {
            editMode = EditMode.Transfer;
            StateHasChanged();
        }
        void HandleFormClose()
        {
            editMode = EditMode.None;
            StateHasChanged();
        }
        async void HandleTransactionSaved(Transaction transaction)
        {
            UnitOfWork!.Process(transaction);
            await UnitOfWork!.SaveChanges();

            LoadTransactions(State!.SelectedUser!);
            StateHasChanged();
        }
    }
    public enum EditMode
    {
        None,
        Deposit,
        Withdraw,
        Transfer
    }

}
