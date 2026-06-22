
using UserAPI.Domain.Entities;
using UserAPI.Application.Interfaces;
using UserAPI.Data;
using Microsoft.EntityFrameworkCore;
using UserAPI.Application.DTOs;

namespace UserAPI.Application.Services;
public class RolesService : IRoleService
{
    
    private readonly AppDbContext _context;

    public RolesService(AppDbContext context){
        _context = context;
    }

    public async Task<IEnumerable<GetRoleResponse>> GetAllRolePermissions(){
        var roles = await _context.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .ToListAsync();
            
        return roles.Select(r => GetRoleResponse.ToResponse(
            r.RoleId,
            r.Name,
            r.RolePermissions.Select(rp => rp.Permission.Name).ToList()
        )).ToList();
    }

    public async Task<GetRoleResponse?> GetAllRolePermissions(int roleId){
        var role = await _context.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.RoleId == roleId);
            
        if (role == null) return GetRoleResponse.ToNotFoundResponse();
        
        return GetRoleResponse.ToResponse(
            role.RoleId,
            role.Name,
            role.RolePermissions.Select(rp => rp.Permission.Name).ToList()
        );
    }

    public async Task<GetRoleResponse?> GetAllRolePermissions(string roleName){
        var role = await _context.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.Name == roleName);
            
        if (role == null) return GetRoleResponse.ToNotFoundResponse();
        
        return GetRoleResponse.ToResponse(
            role.RoleId,
            role.Name,
            role.RolePermissions.Select(rp => rp.Permission.Name).ToList()
        );
    }   

    public async Task<int> CreateRole(CreateRoleRequest request){
        var existingRolePermissions = await _context.Roles.FirstOrDefaultAsync(r => r.Name == request.Name);
        if (existingRolePermissions != null) return existingRolePermissions.RoleId;
        var newRole = new Role { Name = request.Name };
        _context.Roles.Add(newRole);
        await _context.SaveChangesAsync();
        return newRole.RoleId;
    }

    public async Task UpdateRole(Role role){
        _context.Roles.Update(role);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRole(Role role){
        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
    }
}