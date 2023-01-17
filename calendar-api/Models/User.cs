using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace calendar_api.Models
{
    public class User : IdentityUser
    {
        public string PasswordSalt { get; set; } = string.Empty;
        //public byte[] PasswordHash { get; set; }
        //public byte[] PasswordSalt { get; set; }
        //[JsonIgnore]
        //public ICollection<CalendarTask> Tasks { get; set; } = new List<CalendarTask>();
    }
}