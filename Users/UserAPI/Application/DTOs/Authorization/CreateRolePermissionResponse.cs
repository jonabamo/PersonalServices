using UserAPI.Domain.Entities;

namespace UserAPI.Application.DTOs;

public class CreateRolePermissionResponse : BaseResponse
{
    public CreateRolePermissionResponse(bool success, string message, int statusCode, GetRoleResponse ?rolePermissions){
        Success = success;
        Message = message;
        StatusCode = statusCode;
        Role = new PermissionRoleDto
        {
            RoleId = rolePermissions != null ? rolePermissions.RoleId : 0,
            RoleName = rolePermissions != null ? rolePermissions.Name : string.Empty,
            Permissions = rolePermissions != null ? rolePermissions.Permissions : null
        };
    }

    public static CreateRolePermissionResponse ToSuccessResponse(GetRoleResponse ?rolePermissions) => new CreateRolePermissionResponse(
        true, "Role Permission created successfully!", 201, rolePermissions
    );
    
    public static CreateRolePermissionResponse ToRoleNotFoundResponse(int roleId) => new CreateRolePermissionResponse(
       false, "Role Permission creation error, specified Role Id: {" + roleId + "} was not found!", 404, GetRoleResponse.ToNotFoundResponse()
    );

    public static CreateRolePermissionResponse ToPermissionNotFoundResponse(int permissionId) => new CreateRolePermissionResponse(
        false, "Role Permission creation error, specified Permission Id: {" + permissionId + "} was not found!", 404, GetRoleResponse.ToNotFoundResponse()
    );
}