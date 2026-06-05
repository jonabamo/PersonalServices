namespace TaskAPI.Application.DTOs.Requests;

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

public class UpdateUserRequest
{
    [Required(ErrorMessage = "Name required!")]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters long!")]
    public required string Name { get; set; }
    [Required(ErrorMessage = "Email required!")]
    [EmailAddress(ErrorMessage = "Invalid email!")]
    public required string Email { get; set; }
    public required string Role { get; set; }
    public required string Password { get; set;}
}