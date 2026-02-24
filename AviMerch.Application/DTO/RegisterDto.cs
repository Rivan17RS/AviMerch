using System.ComponentModel.DataAnnotations;

namespace AviMerch.Application.DTO;

public class RegisterDto
{
    [Required]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }

    public string? FullName { get; set; }
}