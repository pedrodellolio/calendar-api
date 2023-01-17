using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using calendar_api.Models;

namespace calendar_api.Models.DTOs
{
    public class UserRegistrationDTO
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}