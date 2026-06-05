using UserAPI.Application.DTOs.Requests;
using UserAPI.Application.DTOs.Responses;
using UserAPI.Domain.Entities;

namespace UserAPI.Application.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<GetRoleResponse>> GetAllRoles();
    Task<GetRoleResponse?> GetRoleById(int id);
    Task<GetRoleResponse?> GetRoleByName(string name);
    Task<int> CreateRole(CreateRoleRequest role);
    Task UpdateRole(Role role);
    Task DeleteRole(Role role);
}