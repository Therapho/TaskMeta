using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Interfaces;

public interface IUserService
{
    Task<ApplicationUser> GetCurrentUser();
    Task<bool> IsAdmin();
    Task<bool> IsAdmin(ApplicationUser user);
    Task<bool> IsLoggedIn();
}