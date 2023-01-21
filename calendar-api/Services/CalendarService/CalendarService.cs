using calendar_api.Models;
using calendar_api.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace calendar_api.Services.UserService
{
    public class CalendarService : ICalendarService
    {
        public readonly DataContext _context;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CalendarService(DataContext context, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CalendarTask> GetTaskById(int taskId)
        {
            var user = await _userService.GetLoggedUser();
            return await _context.Tasks.Where(t => t.Id == taskId && t.User == user).FirstOrDefaultAsync();
        }
        public async Task<List<CalendarTask>> GetTasksByDate(DateTime date)
        {
            var user = await _userService.GetLoggedUser();
            return await _context.Tasks.Where(t => (t.StartDate.Day == date.Day) && (t.StartDate.Month == date.Month) && (t.StartDate.Year == date.Year) && (t.User == user)).ToListAsync();
        }

        public async Task CreateTask(CalendarTask task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTask(CalendarTask task)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTask(CalendarTask oldTask, TaskDTO updatedTask)
        {
            oldTask.Title = updatedTask.Title;
            oldTask.Description = updatedTask.Description;
            oldTask.StartDate = DateTimeOffset.FromUnixTimeMilliseconds(updatedTask.StartDateInMilliseconds).LocalDateTime;
            oldTask.EndDate = DateTimeOffset.FromUnixTimeMilliseconds(updatedTask.EndDateInMilliseconds).LocalDateTime;
            await _context.SaveChangesAsync();
        }
    }
}
