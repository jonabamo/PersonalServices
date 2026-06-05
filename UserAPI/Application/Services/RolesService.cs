
using UserAPI.Domain.Entities;
using UserAPI.Application.Interfaces;
using UserAPI.Data;
using Microsoft.EntityFrameworkCore;
using UserAPI.Application.DTOs.Requests;
using UserAPI.Application.DTOs.Responses;

namespace UserAPI.Application.Services;
public class RolesService : IRoleService
{
    
    private readonly AppDbContext _context;

    public RolesService(AppDbContext context){
        _context = context;
    }

    public async Task<IEnumerable<GetRoleResponse>> GetAllRoles(){
        var roles = await _context.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .ToListAsync();
            
        return roles.Select(r => new GetRoleResponse(
            r.RoleId,
            r.Name,
            r.RolePermissions.Select(rp => rp.Permission.Name).ToList()
        )).ToList();
    }

    public async Task<GetRoleResponse?> GetRoleById(int id){
        var role = await _context.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.RoleId == id);
            
        if (role == null) return null;
        
        return new GetRoleResponse(
            role.RoleId,
            role.Name,
            role.RolePermissions.Select(rp => rp.Permission.Name).ToList()
        );
    }

    public async Task<GetRoleResponse?> GetRoleByName(string name){
        var role = await _context.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.Name == name);
            
        if (role == null) return null;
        
        return new GetRoleResponse(
            role.RoleId,
            role.Name,
            role.RolePermissions.Select(rp => rp.Permission.Name).ToList()
        );
    }   

    public async Task<int> CreateRole(CreateRoleRequest request){
        var existingRole = await GetRoleByName(request.Name);
        if (existingRole != null) return existingRole.RoleId;
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