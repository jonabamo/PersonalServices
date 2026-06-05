using TaskAPI.Application.DTOs.Requests;
using TaskAPI.Application.DTOs.Responses;
using TaskAPI.Domain.Entities;

namespace TaskAPI.Application.Interfaces
{
    public interface IPermissionService
    {
        Task<IEnumerable<GetPermisionResponse>> GetAllPermissions();
        Task<GetPermisionResponse> GetPermissionResponseById(int id);
        Task<GetPermisionResponse> GetPermissionResponseByName(string name);
        Task<Permission?> GetPermissionById(int id);
        Task<Permission?> GetPermissionByName(string name);
        Task<int> CreatePermission(CreatePermissionRequest permission);
        Task UpdatePermissionById(int id, CreatePermissionRequest permission);
        Task DeletePermissionById(int id);
    }
}
