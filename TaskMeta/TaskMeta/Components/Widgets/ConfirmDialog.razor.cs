using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace TaskMeta.Components.Widgets;

public partial class ConfirmDialog
{
    [Parameter]
    public string Content { get; set; } = String.Empty;

    [CascadingParameter]
    public FluentDialog? Dialog { get; set; }

    
}
public static class ConfirmDialogExtensions
{
    public async static Task<IDialogReference> ShowConfirmationDialog(this IDialogService dialogService, string title, string message)
    {
        DialogParameters parameters = new()
        {
            Title = title,
            PrimaryAction = "Yes",
            SecondaryAction = "No",
            Width = "500px",
            TrapFocus = true,
            Modal = true,
            PreventScroll = true
        };
        var dialog = await dialogService!.ShowDialogAsync<ConfirmDialog>(message, parameters);
        return dialog;
    }
}

