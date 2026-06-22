using UserAPI.Common;

namespace UserAPI.Application.DTOs;

public class GetRoleResponse : BaseResponse
{
    public int RoleId { get; set; }
    public string Name { get; set; }
    public List<string> Permissions { get; set; }

    public GetRoleResponse (
        bool success,
        int roleId, 
        string name, 
        List<string> permissions,
        string message,
        int statusCode
    ) {
        Success = success;
        RoleId = roleId;
        Name = name;
        Permissions = permissions;
        Message = message;
        StatusCode = statusCode;
    }
    
    public static GetRoleResponse ToResponse (int roleId,string name,List<string> permissions)
    {
        return new GetRoleResponse(
            true,
            roleId,
            name,
            permissions,
            "Permission found",
            200
        );
    }

    public static GetRoleResponse ToNotFoundResponse() => new GetRoleResponse(
        false,
        0,
        String.Empty,
        new List<string>(), 
        ErrorCodes.PermissionNotFound,
        404
    );
}
