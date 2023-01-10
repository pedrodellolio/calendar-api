using System.Text.Json.Serialization;

namespace calendar_api.Models
{
    public class CalendarTask
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate => DateTime.Now;
        public User User { get; set; }
    }
}