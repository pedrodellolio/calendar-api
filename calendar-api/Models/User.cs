using System.Text.Json.Serialization;

namespace calendar_api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        //[JsonIgnore]
        //public ICollection<CalendarTask> Tasks { get; set; }
    }
}