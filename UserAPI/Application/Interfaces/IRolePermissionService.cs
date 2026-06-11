namespace UserAPI.Application.Interfaces;

using UserAPI.Application.DTOs;
using UserAPI.Domain.Entities;

public interface IRolePermissionService
{
    public Task<List<GetRolePermissionResponse>> GetAllRolePermissions();
    public Task<List<GetRolePermissionResponse>> GetRolePermissionById(int roleId);
    public Task<List<GetRolePermissionResponse>> GetRolePermissionByName(string roleName);
    public Task<CreateRolePermissionResponse> CreateRolePermission(int roleId, int permissionId);
    public Task<UpdateRolePermissionResponse> UpdateRolePermission(CreateRolePermissionRequest rolePermission);
    public Task DeleteRolePermission(CreateRolePermissionRequest rolePermission);
}