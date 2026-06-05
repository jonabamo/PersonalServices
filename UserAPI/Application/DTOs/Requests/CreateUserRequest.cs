namespace UserAPI.Application.DTOs.Requests;

using System.ComponentModel.DataAnnotations;

public class CreateUserRequest
{
    [Required(ErrorMessage = "Name required!")]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters long!")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Email required!")]
    [EmailAddress(ErrorMessage = "Invalid email!")]
    public required string Email { get; set; }

    public required string Role { get; set; }

    public required string Password { get; set; }
}