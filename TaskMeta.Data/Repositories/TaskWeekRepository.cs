using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMeta.Data.Models;

namespace TaskMeta.Data.Repositories
{
    public class TaskWeekRepository : EntityRepository<TaskWeek>, ITaskWeekRepository
    {
        public TaskWeekRepository(ApplicationDbContext context, ILogger<EntityRepository<TaskWeek>> logger) : base(context, logger)
        {
        }
        public async Task<TaskWeek> GetCurrent()
        {
            // Get first dateonly of the current week
            var currentDate = DateTime.Now.Date;
            var currentWeekStart = DateOnly.FromDateTime( currentDate.AddDays(-(int)currentDate.DayOfWeek));

            var currentWeek = Context.Set<TaskWeek>().Where(x => x.WeekStartDate == currentWeekStart).FirstOrDefault();

            if (currentWeek == null)
            {
                currentWeek = new TaskWeek()
                {
                    WeekStartDate = currentWeekStart,
                    UserId = ""
                };
                await AddAsync(currentWeek);
            }
            return currentWeek;
        }
    }
}
