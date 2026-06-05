using UserAPI.Application.DTOs.Requests;
using UserAPI.Application.DTOs.Responses;
using UserAPI.Domain.Entities;

namespace UserAPI.Application.Interfaces
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
