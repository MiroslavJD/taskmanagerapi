
using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Dtos.Auth
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = "";

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = "";
    }
}
