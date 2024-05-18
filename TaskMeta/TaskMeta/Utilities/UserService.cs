﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using TaskMeta.Shared.Interfaces;
using TaskMeta.Shared.Models;

namespace TaskMeta.Utilities
{

    public class UserService : IUserService
    {


        AuthenticationStateProvider _authenticationStateProvider;
        UserManager<ApplicationUser> _userManager;
        public UserService(AuthenticationStateProvider authenticationStateProvider, UserManager<ApplicationUser> userManager)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _userManager = userManager;
        }
        public async Task<ApplicationUser> GetCurrentUser()
        {

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();

            if (authState == null)
            {
                throw new InvalidOperationException("Unable to get authentication state.");
            }


            var user = await _userManager.GetUserAsync(authState!.User);
        
            if (user == null)
            {
                throw new InvalidOperationException("Unable to get user.");
            }
            return user;
        }
        public async Task<List<ApplicationUser>> GetContributors()
        {
            var users = await _userManager.GetUsersInRoleAsync("Contributor");
            return users.ToList();
        }
        public async Task<bool> IsAdmin(ApplicationUser user)
        {
            ;
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
            //var user = await _userManager.GetUserAsync(authState!.User);
            return authState!.User != null;
        }
    }
}
