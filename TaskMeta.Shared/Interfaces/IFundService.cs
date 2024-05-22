using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Interfaces
{
    public interface IFundService : IEntityService<Fund>
    {
        Task<List<Fund>> GetFundsByUser(string userId);
    }
}
