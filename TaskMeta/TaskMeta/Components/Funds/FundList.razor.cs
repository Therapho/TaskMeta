using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Components.Funds;

public partial class FundList : ComponentBase, IDisposable
{
    private bool isAdmin;
    private List<ApplicationUser>? contributors;
    private ApplicationUser? selectedUser;
    private List<Fund>? fundList;
    private bool editMode = false;
    

    [SupplyParameterFromForm]
    private Fund? EditFund { get; set; }
  

    [Inject]
    public IUserService? UserService { get; set; }
    [Inject]
    public IFundService? FundService { get; set; }
    [Inject]
    public IDialogService? DialogService { get; set; }
    private EditContext? editContext;
    private ValidationMessageStore? messageStore;

    int fundAllocationTotal;
    private string warningMessage = string.Empty;

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
        editContext = new(new Fund());
        await base.OnInitializedAsync();
    }

    async void HandleUserSelected(ApplicationUser user)
    {
        await LoadFunds(user);

    }

    private async Task LoadFunds(ApplicationUser user)
    {
        selectedUser = user;

        fundList = await FundService!.GetFundsByUser(user.Id);
        RecalculatePage();
    }
    private void HandleAddFund(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
    {
        var fund = new Fund()
        {
            UserId = selectedUser!.Id,
            TargetDate = DateTime.Now.ToDateOnly(),
            Balance = 0,
            TargetBalance = 0,
            Allocation = 0,
            Description = ""

        };
        SetupEdit(fund);
    }
    private void HandleEditFund(Fund fund)
    {
        SetupEdit(fund);
    }
    private async void HandleSaveFund()
    {
        editMode = false;

        if (EditFund == null) throw new NullReferenceException("EditFund is null");

        if (EditFund!.Id == 0)
        {
            await FundService!.AddAsync(EditFund);
            fundList!.Add(EditFund);
        }
        else
        {            
            
            await FundService!.UpdateAsync(EditFund);
        }
        
        ClearEdit();
        RecalculatePage();
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
            fundList!.Remove(fund);
            RecalculatePage();
        }
    }
    public void RecalculatePage()
    {
        fundAllocationTotal = fundList!.Sum(f => f.Allocation.GetValueOrDefault());
        if (fundAllocationTotal != 100) warningMessage = "Fund allocation total is not 100%.";
        else warningMessage = string.Empty;
        StateHasChanged();

    }
    private void HandleCancelEdit()
    {
        editMode = false;
        EditFund = null;
    }
    private void HandleValidationRequested(object? sender,
      ValidationRequestedEventArgs args)
    {
        messageStore?.Clear();

        if(String.IsNullOrEmpty(EditFund?.Name))
        {
            messageStore?.Add(() => EditFund!.Name, "Name is required.");
        }

        var total = 0;
        foreach (var fund in fundList!)
        {
            if(fund.Id == EditFund!.Id)
            {
                total += EditFund.Allocation.GetValueOrDefault();
            }
            else
            {
                total += fund.Allocation.GetValueOrDefault();
            }
        }
        if(total > 100)
        {
            messageStore?.Add(() => EditFund!.Allocation!, "Total allocation cannot exceed 100%.");
        }
 
    }
    private void SetupEdit(Fund fund)
    {
        EditFund = fund;
        editContext = new(EditFund);
        editContext.OnValidationRequested += HandleValidationRequested;
        messageStore = new(editContext);
        editMode = true;
    }
    private void ClearEdit()
    {
        editMode = false;
        if (editContext is not null)
        {
            editContext.OnValidationRequested -= HandleValidationRequested;
        }
        

    }
    public void Dispose()
    {
        ClearEdit();
    }
}