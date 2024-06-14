using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;

namespace TaskMeta.Shared.Models.Repositories
{

    public class UserRepository(AuthenticationStateProvider authenticationStateProvider, UserManager<ApplicationUser> userManager, 
        ICacheProvider cacheProvider) : IUserRepository
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ICacheProvider _cacheProvider = cacheProvider;

        public async Task<ApplicationUser?> GetCurrentUser()
        {
            var user = _cacheProvider.Get<ApplicationUser>("CurrentUser");
            if (user != null) return user;

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync      () ?? throw new InvalidOperationException("Unable to get authentication state.");
            user = await _userManager.GetUserAsync(authState!.User);

            return user;
        }
        public async Task<List<ApplicationUser>> GetContributors()
        {
            var users = _cacheProvider.Get<List<ApplicationUser>>("Contributors");
            if (users != null) return users;
            users = [..await _userManager.GetUsersInRoleAsync("Contributor")];
            return users;
        }
        public async Task<bool> IsAdmin(ApplicationUser user)
        {
            var isAdmin = _cacheProvider.Get<bool?>($"IsAdmin_{user.Id}");
            if (isAdmin != null) return isAdmin.Value;
            return await _userManager.IsInRoleAsync(user, "Admin");
        }
        public async Task<bool> IsAdmin()
        {
            var user = await GetCurrentUser();
            if (user == null) return false;
            return await IsAdmin(user);
        }
        public async Task<bool> IsLoggedIn()
        {
            var user = await GetCurrentUser();
            
            var isLoggedIn = user != null;
            return isLoggedIn;
        }
    }
}
