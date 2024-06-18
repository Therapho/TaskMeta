using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Interfaces;

public interface IUserRepository
{
    Task<ApplicationUser?> GetCurrentUser();
    Task<bool> IsAdmin();
    Task<bool> IsAdmin(ApplicationUser user);
    Task<bool> IsLoggedIn();
    Task<List<ApplicationUser>> GetContributors();
    Task<List<ApplicationUser>>? GetAllUsers();
    Task Add(ApplicationUser user);
    Task Delete(ApplicationUser user);
    Task Update(ApplicationUser user);
    Task<String> ResetPassword(ApplicationUser user);
}