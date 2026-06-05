namespace UserAPI.Application.Interfaces;

using UserAPI.Application.DTOs.Requests;
using UserAPI.Application.DTOs.Responses;
using UserAPI.Domain.Entities;

public interface IRolePermissionService
{
    public Task<List<GetRolePermissionResponse>> GetAllRolePermissions();
    public Task<List<GetRolePermissionResponse>> GetRolePermissionById(int roleId);
    public Task<List<GetRolePermissionResponse>> GetRolePermissionByName(string roleName);
    public Task<GetRolePermissionResponse> CreateRolePermission(int roleId, int permissionId);
    public Task<GetRolePermissionResponse> CreateRolePermission(string roleName, string permissionName);
    public Task UpdateRolePermission(CreateRolePermissionRequest rolePermission);
    public Task DeleteRolePermission(CreateRolePermissionRequest rolePermission);
}