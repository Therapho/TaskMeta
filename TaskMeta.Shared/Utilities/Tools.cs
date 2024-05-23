using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMeta.Shared.Utilities
{
    public static class Tools
    {
        public static DateOnly StartOfWeek(this DateOnly date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Sunday)) % 7;
            return date.AddDays(-1 * diff);
        }
        public static DateOnly StartOfWeek(this DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Sunday)) % 7;
            return date.AddDays(-1 * diff).ToDateOnly();
        }
        public static DateOnly? ToDateOnly(this DateTime? dateTime)
        {
            if (dateTime == null) return null;
            return DateOnly.FromDateTime(dateTime.Value);
            
        }
        public static DateOnly ToDateOnly(this DateTime dateTime)
        {
            return DateOnly.FromDateTime(dateTime);
        }
        public static DateTime? ToDateTime(this DateOnly? dateOnly)
        {
            if (dateOnly == null) return null;
            var result = dateOnly.Value.ToDateTime(TimeOnly.FromDateTime(DateTime.MinValue));
            return result;
        }
        public static DateTime ToDateTime(this DateOnly dateOnly)
        {
            var result = dateOnly.ToDateTime(TimeOnly.FromDateTime(DateTime.MinValue));
            return result;
        }
    }

}
