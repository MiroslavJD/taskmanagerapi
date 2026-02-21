using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Dtos;

public class UpdateUserDto
{
    [Required, MaxLength(50)]
    public string Username { get; set; } = "";
}