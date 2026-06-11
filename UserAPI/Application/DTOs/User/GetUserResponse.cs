using UserAPI.Domain.Entities;
using UserAPI.Common;

namespace UserAPI.Application.DTOs;

public class GetUserResponse : BaseResponse
{
    // public string Name { get; set; } = string.Empty;
    // public string Role { get; set; } = string.Empty;
    // public string Email { get; set; } = string.Empty;

    public GetUserResponse(bool success, string message, User user, GetRoleResponse ?rolePermissions)
    {
        Success = success;
        Message = message;
        StatusCode = StatusCodes.Status200OK;
        User = new UserDto
        {
            Name = user.Name,
            //Role = user.Role,
            Email = user.Email  
        };
        Role = new PermissionRoleDto
        {
            RoleId = rolePermissions != null ? rolePermissions.RoleId : 0,
            RoleName = rolePermissions != null ? rolePermissions.Name : string.Empty,
            Permissions = rolePermissions != null ? rolePermissions.Permissions : null
        };
    }

    public GetUserResponse(bool success, string message, int statusCode)
    {
        Success = success;
        Message = message;
        StatusCode = statusCode;
    }

    public static GetUserResponse ToResponse(User user, GetRoleResponse ?rolePermissions) => new GetUserResponse(
        true,
        "User found",
        user,
        rolePermissions
    );

    public static GetUserResponse ToNotFoundResponse() => new GetUserResponse(
        false,
        ErrorCodes.UserNotFound,
        404
    );
}