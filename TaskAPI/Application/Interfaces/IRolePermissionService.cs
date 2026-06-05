namespace TaskAPI.Application.Interfaces;

using TaskAPI.Application.DTOs.Requests;
using TaskAPI.Application.DTOs.Responses;
using TaskAPI.Domain.Entities;

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