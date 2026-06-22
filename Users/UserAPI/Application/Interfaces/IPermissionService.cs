using UserAPI.Application.DTOs;
using UserAPI.Domain.Entities;

namespace UserAPI.Application.Interfaces
{
    public interface IPermissionService
    {
        Task<IEnumerable<GetPermisionResponse>> GetAllPermissions();
        Task<GetPermisionResponse> GetPermissionByName(string name);
        Task<GetPermisionResponse?> GetPermissionById(int id);
        Task<int> CreatePermission(CreatePermissionRequest permission);
        Task UpdatePermissionById(int id, CreatePermissionRequest permission);
        Task DeletePermissionById(int id);
    }
}
