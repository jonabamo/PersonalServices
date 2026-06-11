namespace UserAPI.Application.DTOs;
using UserAPI.Domain.Entities;
using UserAPI.Common;

public class UpdateRolePermissionResponse : BaseResponse
{
    public UpdateRolePermissionResponse(bool success, string message){
        Success = Success;
        Message = message;
    }

    public static UpdateRolePermissionResponse ToUpdateResponse() => new UpdateRolePermissionResponse(
        true, "Role Permission updated!"
    );

    public static UpdateRolePermissionResponse ToUpdateResponse(string RolePermissionName) => new UpdateRolePermissionResponse(
        true, "Role Permission " + RolePermissionName + " updated!"
    );

    public static UpdateRolePermissionResponse ToNotFoundUpdateResponse() => new UpdateRolePermissionResponse(
        false, ErrorCodes.PermissionNotFound//"Role Permission not found!"
    );

    public static UpdateRolePermissionResponse ToNotFoundUpdateResponse(Guid id) => new UpdateRolePermissionResponse(
        false, "Role Permission with id " + id + " not found!"
    );

    public static UpdateRolePermissionResponse ToNotFoundUpdateResponse(string RolePermissionName) => new UpdateRolePermissionResponse(
        false, "Role Permission " + RolePermissionName + " not found!"
    );

    public static UpdateRolePermissionResponse ToTruncateResponse(string field) => new UpdateRolePermissionResponse(
        false, "The field " + field + " is too long!"
    );
}