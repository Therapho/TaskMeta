using CommunityToolkit.Diagnostics;
using System.Diagnostics;
using TaskMeta.MVVM;
using TaskMeta.Shared.Models;

namespace TaskMeta.Components.Views;

public partial class Summary : ViewBase<SummaryViewModel>
{


    protected override async Task OnInitializedAsync()
    {
        Guard.IsNotNull(ViewModel);
        
        if (!ViewModel.Loaded) await ViewModel.Load();
        await base.OnInitializedAsync();
    }

    
   
}
