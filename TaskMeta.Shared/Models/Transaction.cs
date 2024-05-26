using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMeta.Shared.Models
{
    public class Transaction
    {
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string TargetUserId { get; set; } = string.Empty;
        public string CallingUserId { get; set; } = string.Empty;

        public Fund? SourceFund { get; set; }
        public Fund? TargetFund { get; set; }

    }
}
