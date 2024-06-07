using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;

namespace TaskMeta.Utilities
{

    public class UserRepository(AuthenticationStateProvider authenticationStateProvider, UserManager<ApplicationUser> userManager) : IUserRepository
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<ApplicationUser> GetCurrentUser()
        {

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync() ?? throw new InvalidOperationException("Unable to get authentication state.");
            var user = await _userManager.GetUserAsync(authState!.User);

            return user ?? throw new InvalidOperationException("Unable to get user.");
        }
        public async Task<List<ApplicationUser>> GetContributors()
        {
            var users = await _userManager.GetUsersInRoleAsync("Contributor");
            return [.. users];
        }
        public async Task<bool> IsAdmin(ApplicationUser user)
        {
            
            return await _userManager.IsInRoleAsync(user, "Admin");
        }
        public async Task<bool> IsAdmin()
        {
            var user = await GetCurrentUser();
            return await IsAdmin(user);
        }
        public async Task<bool> IsLoggedIn()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            
            var isLoggedIn = authState != null && authState.User != null && authState.User.Identity != null 
                && authState.User.Identity.IsAuthenticated;
            return isLoggedIn;
        }
    }
}
