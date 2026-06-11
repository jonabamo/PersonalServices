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

    // public static CreateUserResponse ToSuccessResponse(string name) => new CreateUserResponse(
    //    true, "User created successfully! Name: " + name, 201
    // );

    public static CreateUserResponse ToSuccessResponse(string name, string email) => new CreateUserResponse(
        true, "User created successfully! Name: " + name + ", Email: " + email, 201
    );

    // public static CreateUserResponse ToTruncateResponse(string field) => new CreateUserResponse(
    //     false, "The field " + field + " is too long!", 400
    // );

    // public static CreateUserResponse ToDuplicateResponse() => new CreateUserResponse(
    //     false, "Duplicated Information!", 409
    // );
}