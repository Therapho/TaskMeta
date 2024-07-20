using TaskMeta.Components.ViewModels;
using TaskMeta.MVVM;
using TaskMeta.Shared;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Components.Controllers;

public class TransactionController(IUnitOfWork unitOfWork, ApplicationState state, UserSelectorViewModel userSelectorViewModel,
    TransactionListViewModel transactionListViewModel, TransactionFormViewModel transactionFormViewModel) 
    : ControllerBase(unitOfWork, state)
{
   
    public Constants.EditMode EditMode { get; set; } = Constants.EditMode.None;
  
    public UserSelectorViewModel UserSelectorViewModel { get; set; } = userSelectorViewModel;
    public TransactionListViewModel TransactionListViewModel { get; set; } = transactionListViewModel;
    public TransactionFormViewModel TransactionFormViewModel { get; set; } = transactionFormViewModel;
    public override async Task Load()
    {        
        await base.Load();
        if (!State.IsAdmin)
        {
            TransactionListViewModel.Load(State!.CurrentUser!);
            
        }
        else
        {            
            UserSelectorViewModel.Load();
            if (State?.SelectedUser != null)
            {
                TransactionListViewModel.Load(State.SelectedUser);                
            }

        }
        await Task.CompletedTask;
    }
    
    public void HandleUserSelected(ApplicationUser user)
    {
        State!.SelectedUser = user;
        TransactionListViewModel.Load(user);
        EditMode = Constants.EditMode.None;
        StateHasChanged!();
    }

    public void HandleDeposit()
    {
        EditMode = Constants.EditMode.Deposit;

        TransactionFormViewModel.Load(State!.SelectedUser!, EditMode);
        StateHasChanged!();
    }

    public void HandleWithdraw()
    {
        EditMode = Constants.EditMode.Withdraw;
        TransactionFormViewModel.Load(State!.SelectedUser!, EditMode);
        StateHasChanged!();
    }

    public void HandleTransfer()
    {
        EditMode = Constants.EditMode.Transfer;
        TransactionFormViewModel.Load(State!.SelectedUser!, EditMode);
        StateHasChanged!();
    }

    public void HandleFormClose()
    {
        EditMode = Constants.EditMode.None;
        TransactionListViewModel.Load(State.SelectedUser!);
        StateHasChanged!();
    }

   
}


