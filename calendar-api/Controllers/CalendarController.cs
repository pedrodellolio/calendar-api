
using calendar_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace calendar_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalendarController : ControllerBase
    {
        public readonly DataContext _context;
        public CalendarController(DataContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetTasks")]
        public async Task<List<CalendarTask>> GetTasksByDate(DateTime date)
        {
            return await _context.Tasks.Where(t => t.StartDate == date).ToListAsync();
        }

        [HttpPost(Name = "CreateTask")]
        public async Task<CalendarTask> CreateTask(CalendarTask task)
        {
            var loggedUser = await _context.Users.FindAsync(1);
            if (loggedUser != null)
                task.User = loggedUser;

            task.StartDate = DateTime.Now;
            task.EndDate = DateTime.Now;

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        [HttpPut(Name = "UpdateTask")]
        public async Task UpdateTaskById(int taskId, int day)
        {
        }

        [HttpDelete(Name = "DeleteTask")]
        public async Task DeleteTaskById(int taskId, int day)
        {
        }
    }
}