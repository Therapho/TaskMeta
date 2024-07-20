using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Interfaces;

public interface IUserRepository
{
    Task<ApplicationUser?> GetCurrentUser();
    Task<bool> IsAdmin();
    Task<bool> IsAdmin(ApplicationUser user);
    Task<bool> IsLoggedIn();
    List<ApplicationUser> GetContributors();
    List<ApplicationUser>? GetAllUsers();
    void Add(ApplicationUser user);
    void Delete(ApplicationUser user);
    void Update(ApplicationUser user);
    String ResetPassword(ApplicationUser user);
}