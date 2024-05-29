using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TaskMeta.Data.Services;
using TaskMeta.Shared;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;

namespace TaskMeta.Components.Transactions
{
    public partial class TransactionForm : ComponentBase, IDisposable
    {
        [Parameter]
        public ApplicationUser? User { get; set; }

        [Parameter]
        public ApplicationUser? CallingUser { get; set; }

        [Inject]
        public IFundService? FundService { get; set; }

        [Parameter]
        public EventCallback OnClose { get; set; }

        [Parameter]
        public EditMode EditMode { get; set; }

        [Parameter]
        public EventCallback OnSave { get; set; }

        Transaction transaction = new();
        List<Fund>? fundList;

        private EditContext? editContext;
        private ValidationMessageStore? messageStore;

        protected override async Task OnInitializedAsync()
        {
            fundList = await FundService!.GetFundsByUser(User!.Id);
            fundList.Insert(0, new Fund { Id = 0, Name = "Select Fund" });
            //editContext = new(transaction);

            await base.OnInitializedAsync();
        }
        protected override void OnParametersSet()
        {
            SetupForm();
            StateHasChanged();
            base.OnParametersSet();
        }

        private void SetupForm()
        {

            //transaction = new Transaction();
            switch (EditMode)
            {

                case EditMode.Deposit:
                    transaction.CategoryId = Constants.Category.Deposit;
                    break;
                case EditMode.Withdraw:
                    transaction.CategoryId = Constants.Category.Withdrawal;
                    break;
                case EditMode.Transfer:
                    transaction.CategoryId = Constants.Category.Transfer;
                    break;

            }

            transaction.TargetUserId = User!.Id;
            transaction.CallingUserId = CallingUser!.Id;
            editContext = new(transaction);

            editContext.OnValidationRequested += HandleValidationRequested;

            messageStore = new(editContext);
        }
        private void HandleValidationRequested(object? sender, ValidationRequestedEventArgs args)
        {
            messageStore?.Clear();

            if (String.IsNullOrEmpty(transaction?.Description))
            {
                messageStore?.Add(() => transaction!.Description, "Description is required.");
            }
            if (transaction?.Amount <= 0)
            {
                messageStore?.Add(() => transaction!.Amount, "Amount must be greater than 0.");
            }

            if (EditMode == EditMode.Withdraw || EditMode == EditMode.Transfer)
            {
                if (transaction?.SourceFund == null)
                {
                    messageStore?.Add(() => transaction!.SourceFund!, "Source fund is required.");
                }
                var sourceFund = transaction!.SourceFund;

                if (transaction?.Amount > sourceFund!.Balance)
                {
                    messageStore?.Add(() => transaction!.Amount,
                        $"Source fund has {sourceFund.Balance:C}, insufficient to {EditMode} {transaction!.Amount:C}");
                }
            }

            if (EditMode == EditMode.Deposit || EditMode == EditMode.Transfer)
            {
                if (transaction?.TargetFund == null)
                {
                    messageStore?.Add(() => transaction!.TargetFund!, "Target fund is required.");
                }
            }

            if (EditMode == EditMode.Transfer)
            {
                if (transaction?.SourceFund == transaction?.TargetFund)
                {
                    messageStore?.Add(() => transaction!.TargetFund!, "Source and target funds cannot be the same.");
                }
            }
        }

        void HandleTargetFundChanged(string newValue)
        {
            int fundId = int.Parse(newValue);
            var targetFund = fundList!.FirstOrDefault(f => f.Id == fundId);
            transaction.TargetFund = targetFund;
        }
        void HandleSourceFundChanged(string newValue)
        {
            int fundId = int.Parse(newValue);
            var sourceFund = fundList!.FirstOrDefault(f => f.Id == fundId);
            transaction.SourceFund = sourceFund;
        }
        async void HandleSave()
        {
            if (editContext!.Validate())
            {
                await FundService!.Process(transaction);
                await OnClose.InvokeAsync();
                if (OnSave.HasDelegate)
                {
                    await OnSave.InvokeAsync();
                }
            }

        }
        async void HandleCancel()
        {
            await OnClose.InvokeAsync();
        }

        public void Dispose()
        {
            if (editContext != null)
            {
                editContext.OnValidationRequested -= HandleValidationRequested;
            }
            GC.SuppressFinalize(this);
        }
    }

}
