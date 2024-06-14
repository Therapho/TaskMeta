using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Utilities
{
    public class ApplicationState
    {
        public ApplicationUser? SelectedUser { get; set; }
        public ApplicationUser? CurrentUser { get; set; }
        public bool IsAdmin { get; set; }
    }
}
