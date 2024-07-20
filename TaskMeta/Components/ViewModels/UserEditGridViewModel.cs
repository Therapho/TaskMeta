using CommunityToolkit.Diagnostics;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using TaskMeta.MVVM;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;
using TaskMeta.Shared.Utilities;

namespace TaskMeta.Components.ViewModels;

public class UserEditGridViewModel(IUnitOfWork unitOfWork, ApplicationState state) : ViewModelBase(unitOfWork, state), IDisposable
{
    public List<ApplicationUser>? UserList { get; set; }
    public ApplicationUser? EditUser { get; set; }
    public EditContext? EditContext { get; set; } = new(Activator.CreateInstance<ApplicationUser>());

    private ValidationMessageStore? messageStore;

    public void Load()
    {

        UserList = UnitOfWork!.GetAllUsers()!;

        StateHasChanged!();
    }

    private void SetupForm(ApplicationUser User)
    {
        EditContext = new(User);

        EditContext.OnValidationRequested += HandleValidationRequested;

        messageStore = new(EditContext);
    }

    private void HandleValidationRequested(object? sender, ValidationRequestedEventArgs e)
    {
        messageStore?.Clear();

        if (String.IsNullOrEmpty(EditUser?.UserName))
        {
            messageStore?.Add(() => EditUser!.UserName!, "UserName is required.");
        }
        if (String.IsNullOrEmpty(EditUser?.Email))
        {
            messageStore?.Add(() => EditUser!.Email!, "Value is required.");
        }
       
    }

    public void HandleEdit(ApplicationUser user)
    {
        Guard.IsNotNull(user);
        EditUser = user;
        SetupForm(user);
        StateHasChanged!();
    }

    public void HandleDelete(ApplicationUser User)
    {
        UnitOfWork!.DeleteUser(User);
        UserList!.Remove(User);

        StateHasChanged!();
    }

    public void HandleAdd()
    {
        EditUser = Activator.CreateInstance<ApplicationUser>();
        UserList!.Add(EditUser!);
        SetupForm(EditUser!);
        StateHasChanged!();
    }
    public void HandleSave()
    {
        if (String.IsNullOrEmpty(EditUser?.Id))
        {
           UnitOfWork!.AddUser(EditUser!);
        }
        else
        {
            var message = UnitOfWork!.UpdateUser(EditUser!);
            if (!string.IsNullOrEmpty(message)) messageStore?.Add(() => EditUser!.NewPassword!, message);
        }
        EditUser!.NewPassword = string.Empty;
        EditUser = null;
        StateHasChanged!();

    }
    public void HandleCancel()
    {
        if (EditUser != null && String.IsNullOrEmpty(EditUser.Id)) UserList!.Remove(EditUser);
        EditUser = null;
        StateHasChanged!();
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


