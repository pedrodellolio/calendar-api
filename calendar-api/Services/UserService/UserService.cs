using calendar_api.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace calendar_api.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        public UserService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<User?> GetLoggedUser()
        {
            var userId = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
                userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            User? user = null;
            if (userId != null)
                user = await _userManager.FindByIdAsync(userId);

            return user;
        }
    }
}
