using UserAPI.Domain.Entities;

namespace UserAPI.Application.DTOs.Responses;

public class GetUserResponse
{
    //public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    //public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public GetUserResponse(/*Guid id,*/ string name, /*string email,*/ string role, string message)
    {
        //Id = id;
        Name = name;
        //Email = email;
        Role = role;
        Message = message;
    }

    public GetUserResponse(string message)
    {
        Message = message;
    }

    public GetUserResponse(User user)
    {
        //Id = user.Id;
        Name = user.Name;
        //Email = user.Email;
        Role = user.Role;
    }

    public static GetUserResponse ToResponse(User user) => new GetUserResponse(
        //user.Id,
        user.Name,
        //user.Email,
        user.Role,
        "Valid User"
    );

    public static GetUserResponse ToNotFoundUpdateResponse() => new GetUserResponse(
        "User not found!"
    );
}