using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using calendar_api.Models;

namespace calendar_api.Models.DTOs
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public UserTaskDTO? User { get; set; }
    }


    public class UserTaskDTO
    {
        public string Username { get; set; }
        public string Token { get; set; }
    }
}