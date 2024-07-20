using CommunityToolkit.Diagnostics;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
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
        public List<ApplicationUser> GetContributors()
        {
            var users = _cacheProvider.Get<List<ApplicationUser>>("Contributors");
            if (users != null) return users;
            users = [.. Task.Run(()=>_userManager.GetUsersInRoleAsync("Contributor")).Result];
            _cacheProvider.Set("Contributors", users);
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

        public List<ApplicationUser>? GetAllUsers()
        {
            return Task.Run(()=> _userManager.Users.ToListAsync()).Result;
        }
        public void Delete(ApplicationUser user)
        {
            _userManager.DeleteAsync(user);
        }
        public void Update(ApplicationUser user)
        {            
            _userManager.UpdateAsync(user);
        }
        public void Add(ApplicationUser user)
        {
            _userManager.CreateAsync(user);
        
        }
        public string ResetPassword(ApplicationUser user)
        {
            var token = Task.Run(()=> _userManager.GeneratePasswordResetTokenAsync(user)).Result;
            var result = Task.Run(() => _userManager.ResetPasswordAsync(user, token, user.NewPassword!)).Result;
            if (!result.Succeeded)
                return string.Join(", ", result.Errors.Select(e => e.Description));
            else return string.Empty;

        }
    }
}
