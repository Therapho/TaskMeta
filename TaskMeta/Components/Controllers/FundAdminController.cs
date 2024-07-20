using CommunityToolkit.Diagnostics;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;
using TaskMeta.MVVM;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;
using TaskMeta.Components.ViewModels;

namespace TaskMeta.Components.Controllers;

public class FundAdminController(IUnitOfWork unitOfWork, ApplicationState state, IDialogService dialogService, UserSelectorViewModel userSelectorViewModel)
    : ControllerBase(unitOfWork, state), IDisposable
{

    private List<ApplicationUser>? contributors;
    public List<Fund>? FundList;
    public bool EditMode { get; set; }

    public Fund? EditFund { get; set; }

    public IDialogService? DialogService { get; set; } = dialogService;

    public EditContext? EditContext { get; set; } = new(new Fund());
    private ValidationMessageStore? messageStore;
    public int FundAllocationTotal { get; set; }
    public string WarningMessage { get; set; } = string.Empty;
    public UserSelectorViewModel UserSelectorViewModel { get; set; } = userSelectorViewModel;

    public async override Task Load()
    {
        await base.Load();
        if (!State.IsAdmin)
        {
            
            LoadFunds(State!.CurrentUser!);
        }
        else
        {
            //contributors = UnitOfWork!.GetContributors();
            UserSelectorViewModel!.Load();
            if (State?.SelectedUser != null) LoadFunds(State.SelectedUser);
        }
    }

    public void HandleUserSelected(ApplicationUser user)
    {
        State!.SelectedUser = user;
        LoadFunds(user);

    }

    private void LoadFunds(ApplicationUser user)
    {
        Guard.IsNotNull(user);

        FundList = UnitOfWork!.GetFundsByUser(user.Id);
        RecalculatePage();
    }
    public void HandleAddFund(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
    {
        var fund = new Fund()
        {
            UserId = State!.SelectedUser!.Id,
            TargetDate = DateTime.Now.ToDateOnly(),
            Balance = 0,
            TargetBalance = 0,
            Allocation = 0,
            Description = ""

        };
        FundList!.Add(fund);
        SetupEdit(fund);
    }
    public void HandleEditFund(Fund fund)
    {
        SetupEdit(fund);
    }
    public void HandleSaveFund()
    {
        EditMode = false;

        if (EditFund!.Id == 0)
        {
            UnitOfWork!.AddFund(EditFund);
        }
        else
        {

            UnitOfWork!.UpdateFund(EditFund);
        }

        ClearEdit();
        RecalculatePage();
    }
    public async void HandleDeleteFund(Fund fund)
    {
        if (fund.Locked)
        {
            return;
        }


        var dialog = await DialogService!.ShowConfirmationAsync("Delete fund?");
        DialogResult? result = await dialog.Result;
        if (result != null && !result.Cancelled)
        {
            UnitOfWork!.DeleteFund(fund);
            FundList!.Remove(fund);
            RecalculatePage();
        }
    }
    public void RecalculatePage()
    {
        FundAllocationTotal = FundList!.Sum(f => f.Allocation.GetValueOrDefault());
        if (FundAllocationTotal != 100) WarningMessage = "Fund allocation total is not 100%.";
        else WarningMessage = string.Empty;
        StateHasChanged!();

    }
    public void HandleCancelEdit()
    {
        EditMode = false;
        if (EditFund!.Id == 0) FundList!.Remove(EditFund);

        EditFund = null;
    }
    private void HandleValidationRequested(object? sender,
      ValidationRequestedEventArgs args)
    {
        messageStore?.Clear();

        if (string.IsNullOrEmpty(EditFund?.Name))
        {
            messageStore?.Add(() => EditFund!.Name, "Name is required.");
        }

        var total = 0;
        foreach (var fund in FundList!)
        {
            if (fund.Id == EditFund!.Id)
            {
                total += EditFund.Allocation.GetValueOrDefault();
            }
            else
            {
                total += fund.Allocation.GetValueOrDefault();
            }
        }
        if (total > 100)
        {
            messageStore?.Add(() => EditFund!.Allocation!, "Total allocation cannot exceed 100%.");
        }

    }
    private void SetupEdit(Fund fund)
    {
        EditFund = fund;
        EditContext = new(EditFund);
        EditContext.OnValidationRequested += HandleValidationRequested;
        messageStore = new(EditContext);
        EditMode = true;
    }
    private void ClearEdit()
    {
        EditMode = false;
        if (EditContext is not null)
        {
            EditContext.OnValidationRequested -= HandleValidationRequested;
        }


    }
    public void Dispose()
    {
        ClearEdit();
        GC.SuppressFinalize(this);
    }
}
