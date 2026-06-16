using UserAPI.Domain.Entities;

namespace UserAPI.Application.DTOs;

public class CreateUserResponse : BaseResponse
{
    public CreateUserResponse(bool success, string message, User user, GetRoleResponse ?rolePermissions){
        Success = success;
        Message = message;
        StatusCode = StatusCodes.Status200OK;
        User = new UserDto
        {
            Name = user.Name,
            Email = user.Email
        };
        Role = new PermissionRoleDto
        {
            RoleId = rolePermissions != null ? rolePermissions.RoleId : 0,
            RoleName = rolePermissions != null ? rolePermissions.Name : string.Empty,
            Permissions = rolePermissions != null ? rolePermissions.Permissions : null
        };
    }

    public CreateUserResponse(bool success, string message, int statusCode){
        Success = success;
        Message = message;
        StatusCode = statusCode;
    }

    public static CreateUserResponse ToResponse(User user, GetRoleResponse ?rolePermissions) => new CreateUserResponse(
        true, "User created", user, rolePermissions
    );

    public static CreateUserResponse ToErrorResponse() => new CreateUserResponse(
        
        false, "Error creating user(s)!", 400
    );

}