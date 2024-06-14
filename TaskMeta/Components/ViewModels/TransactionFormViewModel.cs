using Microsoft.AspNetCore.Components.Forms;
using TaskMeta.MVVM;
using TaskMeta.Shared;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Components.ViewModels
{
    public class TransactionFormViewModel(IUnitOfWork unitOfWork, ApplicationState state) : ViewModelBase(unitOfWork, state), IDisposable
    {

        public Action? OnClose { get; set; }
        public Constants.EditMode EditMode { get; set; }
        public Action<Transaction>? OnSave { get; set; }
        public Transaction? Transaction;
        public List<Fund>? FundList { get; set; }

        public EditContext? EditContext;
        private ValidationMessageStore? messageStore;

        public async Task Load(ApplicationUser selectedUser, Constants.EditMode editMode)
        {
            FundList = await UnitOfWork!.FundRepository!.GetFundsByUser(State!.SelectedUser!.Id);
            FundList!.Insert(0, new Fund { Id = 0, Name = "Select Fund" });
            EditMode = editMode;
            TearDownForm();
            SetupForm();
        }

        private void SetupForm()
        {

            Transaction = new Transaction();
            switch (EditMode)
            {

                case Constants.EditMode.Deposit:
                    Transaction.CategoryId = Constants.Category.Deposit;
                    break;
                case Constants.EditMode.Withdraw:
                    Transaction.CategoryId = Constants.Category.Withdrawal;
                    break;
                case Constants.EditMode.Transfer:
                    Transaction.CategoryId = Constants.Category.Transfer;
                    break;

            }

            Transaction.TargetUserId = State.SelectedUser!.Id;
            Transaction.CallingUserId = State.CurrentUser!.Id;
            EditContext = new(Transaction);

            EditContext.OnValidationRequested += HandleValidationRequested;

            messageStore = new(EditContext);
        }
        public void HandleValidationRequested(object? sender, ValidationRequestedEventArgs args)
        {
            messageStore?.Clear();

            if (string.IsNullOrEmpty(Transaction?.Description))
            {
                messageStore?.Add(() => Transaction!.Description, "Description is required.");
            }
            if (Transaction?.Amount <= 0)
            {
                messageStore?.Add(() => Transaction!.Amount, "Amount must be greater than 0.");
            }

            if (EditMode == Constants.EditMode.Withdraw || EditMode == Constants.EditMode.Transfer)
            {
                if (Transaction?.SourceFund == null)
                {
                    messageStore?.Add(() => Transaction!.SourceFund!, "Source fund is required.");
                }
                var sourceFund = Transaction!.SourceFund;

                if (Transaction?.Amount > sourceFund!.Balance)
                {
                    messageStore?.Add(() => Transaction!.Amount,
                        $"Source fund has {sourceFund.Balance:C}, insufficient to {EditMode} {Transaction!.Amount:C}");
                }
            }

            if (EditMode == Constants.EditMode.Deposit || EditMode == Constants.EditMode.Transfer)
            {
                if (Transaction?.TargetFund == null)
                {
                    messageStore?.Add(() => Transaction!.TargetFund!, "Target fund is required.");
                }
            }

            if (EditMode == Constants.EditMode.Transfer)
            {
                if (Transaction?.SourceFund == Transaction?.TargetFund)
                {
                    messageStore?.Add(() => Transaction!.TargetFund!, "Source and target funds cannot be the same.");
                }
            }
        }

        public void HandleTargetFundChanged(string newValue)
        {
            int fundId = int.Parse(newValue);
            var targetFund = FundList!.FirstOrDefault(f => f.Id == fundId);
            Transaction!.TargetFund = targetFund;
        }

        public void HandleSourceFundChanged(string newValue)
        {
            int fundId = int.Parse(newValue);
            var sourceFund = FundList!.FirstOrDefault(f => f.Id == fundId);
            Transaction!.SourceFund = sourceFund;
        }

        public async void HandleSave()
        {
            if (EditContext!.Validate())
            {
                UnitOfWork!.Process(Transaction!);
                await UnitOfWork!.SaveChanges();

                TearDownForm();
                OnClose?.Invoke();
            }

        }

        public void HandleCancel()
        {
            TearDownForm();
            OnClose?.Invoke();
        }

        private void TearDownForm()
        {             
            if (EditContext != null)
            {
                EditContext.OnValidationRequested -= HandleValidationRequested;
            }
        }

        public void Dispose()
        {
            TearDownForm();
            GC.SuppressFinalize(this);
        }
    }
}
