using calendar_api.Models;
using calendar_api.Models.DTOs;

namespace calendar_api.Services.UserService
{
    public interface ICalendarService
    {
        Task<CalendarTask> GetTaskById(int taskId);
        Task<List<CalendarTask>> GetTasksByDate(DateTime date);
        Task CreateTask(CalendarTask task);
        Task UpdateTask(CalendarTask oldTask, TaskDTO updatedTask);
        Task DeleteTask(CalendarTask task);

    }
}
