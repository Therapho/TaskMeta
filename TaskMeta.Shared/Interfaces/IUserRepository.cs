using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Interfaces;

public interface IUserRepository
{
    Task<ApplicationUser?> GetCurrentUser();
    Task<bool> IsAdmin();
    Task<bool> IsAdmin(ApplicationUser user);
    Task<bool> IsLoggedIn();
    Task<List<ApplicationUser>> GetContributors();
}