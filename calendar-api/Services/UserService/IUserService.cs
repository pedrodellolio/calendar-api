using calendar_api.Models;

namespace calendar_api.Services.UserService
{
    public interface IUserService
    {
        Task<User?> GetLoggedUser();
    }
}
