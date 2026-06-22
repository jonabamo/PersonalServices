namespace UserAPI.Application.DTOs;
using UserAPI.Domain.Entities;
using UserAPI.Common;

public class UpdateUserResponse : BaseResponse
{
    public UpdateUserResponse(bool success, string message){
        Success = success;
        Message = message;
        StatusCode = StatusCodes.Status200OK;
    }

    public UpdateUserResponse(bool success, string message, int statusCode){
        Success = success;
        Message = message;
        StatusCode = statusCode;
    }

    public static UpdateUserResponse ToUpdateResponse() => new UpdateUserResponse(
        true, "User updated!"
    );

    public static UpdateUserResponse ToUpdateResponse(string userName) => new UpdateUserResponse(
        true, "User " + userName + " updated!", 200
    );

    public static UpdateUserResponse ToNotFoundUpdateResponse() => new UpdateUserResponse(
        false, "User not found!", 404
    );

    public static UpdateUserResponse ToNotFoundUpdateResponse(string userName) => new UpdateUserResponse(
        false, "User " + userName + " not found!", 404
    );

    public static UpdateUserResponse ToConflictResponse(string message = "User was modified by another user. Please reload and try again.") => new UpdateUserResponse(
        false, message, 409
    );

    public static UpdateUserResponse ToErrorResponse(string message = "Error updating user") => new UpdateUserResponse(
        false, message, 500
    );
}