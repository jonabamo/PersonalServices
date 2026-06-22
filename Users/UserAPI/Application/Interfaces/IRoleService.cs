using UserAPI.Application.DTOs;
using UserAPI.Domain.Entities;

namespace UserAPI.Application.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<GetRoleResponse>> GetAllRolePermissions();
    Task<GetRoleResponse?> GetAllRolePermissions(int roleId);
    Task<GetRoleResponse?> GetAllRolePermissions(string roleName);
    Task<int> CreateRole(CreateRoleRequest role);
    Task UpdateRole(Role role);
    Task DeleteRole(Role role);
}