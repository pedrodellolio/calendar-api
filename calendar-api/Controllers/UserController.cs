
using calendar_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace calendar_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public readonly DataContext _context;
        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetUser")]
        public async Task<User?> GetUserById(int userId)
        {
            return await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
        }
    }
}