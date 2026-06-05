namespace UserAPI.Application.DTOs.Responses;

public class GetRolePermissionResponse{
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public int PermissionId { get; set; }
    public string PermissionName { get; set; }

    public GetRolePermissionResponse(int roleId, string roleName, int permissionId, string permissionName) {
        RoleId = roleId;
        RoleName = roleName;
        PermissionId = permissionId;
        PermissionName = permissionName;
    }

    public static List<GetRolePermissionResponse> ToNotFoundResponse(string message)
    {
        return new List<GetRolePermissionResponse> 
        { 
            new GetRolePermissionResponse(0, message, 0, message) 
        };
    }
}
