using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Dtos;

public class CreateUserDto
{
    [Required, MaxLength(50)]
    public string Username { get; set; } = "";
}
