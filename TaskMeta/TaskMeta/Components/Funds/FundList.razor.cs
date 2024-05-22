using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using TaskMeta.Components.Widgets;
using TaskMeta.Data.Services;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Components.Funds;

public partial class FundList : ComponentBase
{
    private bool isAdmin;
    private List<ApplicationUser>? contributors;
    private ApplicationUser? selectedUser;
    private List<Fund>? funds;
    private bool editMode = false;
    private Fund? editFund;

    [SupplyParameterFromForm]
    private FundDTO? fundDTO { get; set; }
  

    [Inject]
    public IUserService? UserService { get; set; }
    [Inject]
    public IFundService? FundService { get; set; }
    [Inject]
    public IDialogService? DialogService { get; set; }
    private EditContext? editContext;

    protected override async Task OnInitializedAsync()
    {
        var user = await UserService!.GetCurrentUser();
        if (user == null) throw new InvalidOperationException("User is null");
        isAdmin = await UserService!.IsAdmin(user);
        if (!isAdmin)
        {

           await LoadFunds(user);
        }
        else
        {
            contributors = await UserService!.GetContributors();

        }

        await base.OnInitializedAsync();
    }

    async void HandleUserSelected(ApplicationUser user)
    {
        await LoadFunds(user);

    }

    private async Task LoadFunds(ApplicationUser user)
    {
        selectedUser = user;

        funds = await FundService!.GetFundsByUser(user.Id);
    }
    private void HandleAddFund(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
    {
        fundDTO = new FundDTO()
        {
            UserId = selectedUser!.Id,
            TargetDate = Tools.Today,
            Balance = 0,
            TargetBalance = 0,
            Allocation = 0,
            Description = ""

        };
        editMode = true;
    }
    private void HandleEditFund(Fund fund)
    {
        editFund = fund;

        fundDTO = new FundDTO()
        {
            Id = fund.Id,
            Name = fund.Name,   
            UserId = fund.UserId,
            TargetDate = fund.TargetDate,
            Balance = fund.Balance,
            TargetBalance = fund.TargetBalance,
            Allocation = fund.Allocation,
            Description = fund.Description
        };
        editMode = true;
    }
    private async void HandleSaveFund()
    {
        editMode = false;

        if (fundDTO != null || fundDTO!.Id == 0)
        {
            await FundService!.AddAsync(fundDTO);
            funds!.Add(fundDTO);
        }
        else
        {            
            editFund!.Name = fundDTO!.Name;
            editFund.Description = fundDTO!.Description;
            editFund.Balance = fundDTO!.Balance;
            editFund.TargetBalance = fundDTO!.TargetBalance;
            editFund.Allocation = fundDTO!.Allocation;
            editFund.TargetDate = fundDTO!.TargetDate;

            await FundService!.UpdateAsync(editFund);
        }      
        StateHasChanged();
    }
    private async void HandleDeleteFund(Fund fund)
    {
        if(fund.Locked)
        {
            return;
        }
      

        var dialog = await DialogService!.ShowConfirmationAsync("Delete fund?");
        DialogResult? result = await dialog.Result;
        if (result != null && !result.Cancelled)
        {
            await FundService!.DeleteAsync(fund.Id);
            funds!.Remove(fund);
            StateHasChanged();
        }
    }
    private void HandleCancelEdit(Fund fund)
    {
        editMode = false;
        fundDTO = null;
    }
  
}