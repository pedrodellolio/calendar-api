
using calendar_api.Models;
using calendar_api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace calendar_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CalendarController : ControllerBase
    {
        public readonly DataContext _context;
        public CalendarController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<CalendarTask>> GetTasksByDate(DateTime date)
        {
            var results = await _context.Tasks.Where(t => (t.StartDate.Day == date.Day) && (t.StartDate.Month == date.Month) && (t.StartDate.Year == date.Year)).ToListAsync();
            return results;
        }

        [HttpGet("id/{taskId}")]
        public async Task<ActionResult<CalendarTask>> GetTasksById(int taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
                BadRequest("Task not found");

            return Ok(task);
        }

        [HttpPost(Name = "CreateTask")]
        public async Task<CalendarTask> CreateTask(TaskDTO req)
        {
            var user = await _context.Users.Where(u => u.UserName == req.User.Username).FirstOrDefaultAsync();
            var task = new CalendarTask
            {
                Description = req.Description,
                StartDate = req.StartDate,
                EndDate = req.EndDate,
                Title = req.Title,
                User = user
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        [HttpPut(Name = "UpdateTask")]
        public async Task<ActionResult<CalendarTask>> UpdateTask(CalendarTask task)
        {
            var dbTask = await _context.Tasks.FindAsync(task.Id);

            if (dbTask == null)
                BadRequest("Task not found");
            else
            {
                dbTask.Title = task.Title;
                dbTask.Description = task.Description;
                dbTask.StartDate = task.StartDate;
                dbTask.EndDate = task.EndDate;
            }

            await _context.SaveChangesAsync();
            return Ok(dbTask);
        }

        [HttpDelete(Name = "DeleteTask")]
        public async Task<ActionResult> DeleteTaskById(int taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);

            if (task == null)
                return BadRequest("Task not found");

            _context.Tasks.Remove(task);
            return Ok();
        }
    }
}