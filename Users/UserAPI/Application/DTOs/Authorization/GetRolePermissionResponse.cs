using UserAPI.Common;

namespace UserAPI.Application.DTOs;

public class GetRolePermissionResponse : BaseResponse
{
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public int PermissionId { get; set; }
    public string PermissionName { get; set; }

    public GetRolePermissionResponse(
        bool success,
        int roleId, 
        string roleName, 
        int permissionId, 
        string permissionName,
        string message,
        int statusCode
    ) {
        Success = success;
        RoleId = roleId;
        RoleName = roleName;
        PermissionId = permissionId;
        PermissionName = permissionName;
        Message = message;
        StatusCode = statusCode;
    }

    public static GetRolePermissionResponse ToResponse(int roleId, string roleName, int permissionId, string permissionName)
    {
        return new GetRolePermissionResponse(
            true, 
            roleId, 
            roleName, 
            permissionId, 
            permissionName,
            "Permission found",
            200);
    }

    public static GetRolePermissionResponse ToNotFoundResponse() => new GetRolePermissionResponse(
        false, 
        0,
        string.Empty,
        0,
        string.Empty,
        ErrorCodes.PermissionNotFound, 
        404
    );

}
