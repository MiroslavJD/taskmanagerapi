using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models
{
    public class TodoTask
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = "";

        public bool IsDone { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;


    }
}
