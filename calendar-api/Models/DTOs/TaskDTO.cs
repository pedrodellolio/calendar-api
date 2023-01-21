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
        public long StartDateInMilliseconds { get; set; }
        public long EndDateInMilliseconds { get; set; }
    }
}