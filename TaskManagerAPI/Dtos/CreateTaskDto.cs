using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Dtos
{
    public class CreateTaskDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = "";
    }
}
