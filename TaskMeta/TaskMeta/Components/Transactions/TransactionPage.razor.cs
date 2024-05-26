using Microsoft.AspNetCore.Components;
using TaskMeta.Data.Services;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Components.Transactions
{

    public partial class TransactionPage :ComponentBase
    {
        [Inject]
        public IUserService? UserService { get; set; }

        [Inject]
        public State? State { get; set; }

        [Inject]
        public ITransactionLogService? TransactionLogService { get; set; }

        [Inject]
        public IFundService? FundService { get; set; }

        private bool isAdmin;
        private List<ApplicationUser>? contributorList;
        private IQueryable<TransactionLog>? transactionQuery;


        EditMode editMode = EditMode.None;
        private ApplicationUser? loggedInUser;

        protected override async Task OnInitializedAsync()
        {
            
            loggedInUser = await UserService!.GetCurrentUser();
            if (loggedInUser == null) throw new InvalidOperationException("User is null");
            isAdmin = await UserService!.IsAdmin(loggedInUser);
            if (!isAdmin)
            {
               LoadTransactions(loggedInUser);
            }
            else
            {
                contributorList = await UserService!.GetContributors();
                if (State?.SelectedUser != null)
                {
                    LoadTransactions(State.SelectedUser);
                }
            }

            await base.OnInitializedAsync();
        }

        private void LoadTransactions(ApplicationUser user)
        {
            transactionQuery = TransactionLogService!.QueryTransactionsByUser(user.Id);
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
        void HandleTransactionSaved()
        {
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
