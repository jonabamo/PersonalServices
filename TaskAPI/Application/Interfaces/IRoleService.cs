using TaskAPI.Application.DTOs.Requests;
using TaskAPI.Application.DTOs.Responses;
using TaskAPI.Domain.Entities;

namespace TaskAPI.Application.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<GetRoleResponse>> GetAllRoles();
    Task<GetRoleResponse?> GetRoleById(int id);
    Task<GetRoleResponse?> GetRoleByName(string name);
    Task<int> CreateRole(CreateRoleRequest role);
    Task UpdateRole(Role role);
    Task DeleteRole(Role role);
}