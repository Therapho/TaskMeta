using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using TaskMeta.Shared.Models;

namespace TaskMeta.Components.Transactions
{
    public partial class TransactionList : ComponentBase
    {
        [Parameter]
        public IQueryable<TransactionLog>? Source { get; set; }
        PaginationState pagination = new PaginationState { ItemsPerPage = 10 };

    }
}
