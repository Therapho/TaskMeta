using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMeta.Shared.Utilities
{
    public static class Tools
    {
        public static DateOnly StartOfWeek
        {
            get
            {
                var dt = DateTime.Now;
                int diff = (7 + (dt.DayOfWeek - DayOfWeek.Sunday)) % 7;
                return DateOnly.FromDateTime(dt.AddDays(-1 * diff).Date);
            }
        }
        public static DateOnly Today
        {
            get
            {
                var dt = DateTime.Now;
                return DateOnly.FromDateTime(dt);
            }
        }
    }
}
