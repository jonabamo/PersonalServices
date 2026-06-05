namespace UserAPI.Application.DTOs.Responses;
using UserAPI.Domain.Entities;

public class UpdateUserResponse
{
    public string Message { get; set; } = string.Empty;

    public UpdateUserResponse(string message){
        Message = message;
    }

    public static UpdateUserResponse ToUpdateResponse() => new UpdateUserResponse(
        "User updated!"
    );

    public static UpdateUserResponse ToUpdateResponse(string userName) => new UpdateUserResponse(
        "User " + userName + " updated!"
    );

    public static UpdateUserResponse ToNotFoundUpdateResponse() => new UpdateUserResponse(
        "User not found!"
    );

    public static UpdateUserResponse ToNotFoundUpdateResponse(Guid id) => new UpdateUserResponse(
        "User with id " + id + " not found!"
    );

    public static UpdateUserResponse ToNotFoundUpdateResponse(string userName) => new UpdateUserResponse(
        "User " + userName + " not found!"
    );

    public static UpdateUserResponse ToTruncateResponse(string field) => new UpdateUserResponse(
        "The field " + field + " is too long!"
    );
}