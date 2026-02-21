using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Dtos
{
    public class UpdateTaskDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = "";

        public bool IsDone { get; set; }
    }
}
