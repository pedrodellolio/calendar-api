
using calendar_api.Models;
using calendar_api.Models.DTOs;
using calendar_api.Services.UserService;
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
        private readonly IUserService _userService;
        private readonly ICalendarService _calendarService;

        public readonly DataContext _context;
        public CalendarController(DataContext context, IUserService userService, ICalendarService calendarService)
        {
            _userService = userService;
            _calendarService = calendarService;
            _context = context;
        }

        [HttpGet("date/{date}")]
        public async Task<List<TaskDTO>> GetTasksByDate(DateTime date)
        {
            var tasks = new List<TaskDTO>();
            var results = await _calendarService.GetTasksByDate(date);
            results.ForEach(task =>
            {
                tasks.Add(new TaskDTO
                {
                    Id = task.Id,
                    Description = task.Description,
                    Title = task.Title,
                    StartDateInMilliseconds = task.StartDate.Millisecond,
                    EndDateInMilliseconds = task.EndDate.Millisecond
                });

            });
            return tasks;
        }

        [HttpGet("id/{taskId}")]
        public async Task<ActionResult<TaskDTO>> GetTaskById(int taskId)
        {
            var taskDB = await _calendarService.GetTaskById(taskId);
            if (taskDB == null)
                BadRequest("Task not found");

            var task = new TaskDTO
            {
                Id = taskDB.Id,
                Description = taskDB.Description,
                Title = taskDB.Title,
                StartDateInMilliseconds = new DateTimeOffset(taskDB.StartDate).ToUnixTimeMilliseconds(),
                EndDateInMilliseconds = new DateTimeOffset(taskDB.EndDate).ToUnixTimeMilliseconds()
            };
            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<CalendarTask>> CreateTask(TaskDTO req)
        {
            var user = await _userService.GetLoggedUser();
            var task = new CalendarTask
            {
                Description = req.Description,
                StartDate = DateTimeOffset.FromUnixTimeMilliseconds(req.StartDateInMilliseconds).LocalDateTime,
                EndDate = DateTimeOffset.FromUnixTimeMilliseconds(req.EndDateInMilliseconds).LocalDateTime,
                Title = req.Title,
                User = user!
            };

            await _calendarService.CreateTask(task);
            return Ok(task);
        }

        [HttpPut]
        public async Task<ActionResult<CalendarTask>> UpdateTask(TaskDTO task)
        {
            var dbTask = await _context.Tasks.FindAsync(task.Id);
            if (dbTask == null)
                BadRequest("Task not found");
            else
                await _calendarService.UpdateTask(dbTask, task);

            return Ok(dbTask);
        }

        [HttpDelete("id/{taskId}")]
        public async Task<ActionResult> DeleteTaskById(int taskId)
        {
            var task = await _calendarService.GetTaskById(taskId);

            if (task == null)
                return BadRequest("Task not found");

            await _calendarService.DeleteTask(task);
            return Ok();
        }
    }
}