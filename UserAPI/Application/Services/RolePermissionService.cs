namespace UserAPI.Application.Services;

using UserAPI.Domain.Entities;
using UserAPI.Application.Interfaces;
using UserAPI.Data;
using Microsoft.EntityFrameworkCore;
using UserAPI.Application.DTOs.Requests;
using UserAPI.Application.DTOs.Responses;

public class RolePermissionService : IRolePermissionService
{
    private readonly AppDbContext _context;
    private readonly IRoleService _roleService;
    private readonly IPermissionService _permissionService;

    public RolePermissionService(AppDbContext context, IRoleService rolesService, IPermissionService permissionService){
        _context = context;
        _roleService = rolesService;
        _permissionService = permissionService;
    }

    public async Task<List<GetRolePermissionResponse>> GetAllRolePermissions(){
        var rolePermissions = await _context.RolePermissions.Include(rp => rp.Role).Include(rp => rp.Permission).OrderBy(rp => rp.Role.Name).ToListAsync();
        if(rolePermissions == null) return new List<GetRolePermissionResponse>();
        return rolePermissions.Select(rp => new GetRolePermissionResponse(rp.RoleId, rp.Role.Name, rp.PermissionId, rp.Permission.Name)).ToList();
    }

    public async Task<List<GetRolePermissionResponse>> GetRolePermissionById(int roleId){
        var rolePermissions = await _context.RolePermissions.Include(rp => rp.Role).Include(rp => rp.Permission).OrderBy(rp => rp.Role.Name).ToListAsync();
        if (rolePermissions == null) return new List<GetRolePermissionResponse>();
        return rolePermissions.Where(rp => rp.Role.RoleId == roleId).Select(rp => new GetRolePermissionResponse(rp.RoleId, rp.Role.Name, rp.PermissionId, rp.Permission.Name)).ToList();
    }

    public async Task<List<GetRolePermissionResponse>> GetRolePermissionByName(string roleName){
        var rolePermissions = await _context.RolePermissions.Include(rp => rp.Role).Include(rp => rp.Permission).OrderBy(rp => rp.Permission.Name).ToListAsync();
        if(rolePermissions == null) return new List<GetRolePermissionResponse>();
        return rolePermissions.Where(rp => rp.Role.Name == roleName).Select(rp => new GetRolePermissionResponse(rp.RoleId, rp.Role.Name, rp.PermissionId, rp.Permission.Name)).ToList();
    }

    public async Task<GetRolePermissionResponse> CreateRolePermission(int roleId, int permissionId){
        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.RoleId == roleId);
        
        if(role == null){
            throw new Exception("Role not found, verify the data sent");
        }

        var permission = await _context.Permissions
            .FirstOrDefaultAsync(p => p.PermissionId == permissionId);
        
        if(permission == null){
            throw new Exception("Permission not found, verify the data sent");
        }

        if(await _context.RolePermissions.AnyAsync(rp => rp.RoleId == role.RoleId && rp.PermissionId == permission.PermissionId)){
            throw new Exception($"Role '{role.Name}' already has permission '{permission.Name}'.");
        }

        var newRolePermission = new RolePermission
        {
            RoleId = role.RoleId,
            PermissionId = permission.PermissionId
        };

        _context.RolePermissions.Add(newRolePermission);
        await _context.SaveChangesAsync();
        return new GetRolePermissionResponse(role.RoleId, role.Name, permission.PermissionId, permission.Name);
    }

    public async Task<GetRolePermissionResponse> CreateRolePermission(string roleName, string permissionName){
        var existingRole = await _context.Roles
        .FirstOrDefaultAsync(r => r.Name == roleName);

        if (existingRole == null)
        {
            await _roleService.CreateRole(new CreateRoleRequest
            {
                Name = roleName
            });
        }

        var existingPermission = await _context.Permissions
        .FirstOrDefaultAsync(p => p.Name == permissionName);

        if (existingPermission == null)
        {
            await _permissionService.CreatePermission(new CreatePermissionRequest
            {
                Name = permissionName
            });
        }

        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == roleName);
        var permission = await _context.Permissions
            .FirstOrDefaultAsync(p => p.Name == permissionName);

        if (role == null || permission == null)
        {
            throw new Exception("Role or permission not found. Please verify the data sent.");
        }
        
        if (await _context.RolePermissions.AnyAsync(rp => rp.RoleId == role.RoleId && rp.PermissionId == permission.PermissionId))
        {
            throw new Exception($"Role '{role.Name}' already has permission '{permission.Name}'.");
        }

        var newRolePermission = new RolePermission
        {
            RoleId = role.RoleId,
            PermissionId = permission.PermissionId
        };

        _context.RolePermissions.Add(newRolePermission);
        await _context.SaveChangesAsync();
        return new GetRolePermissionResponse(role.RoleId, role.Name, permission.PermissionId, permission.Name);
    }

    public async Task UpdateRolePermission(CreateRolePermissionRequest rolePermission){
        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.RoleId == rolePermission.RoleId);

        if (role == null)
        {
            throw new Exception("Role not found, verify the data sent");
        }
        var permission = await _context.Permissions
            .FirstOrDefaultAsync(p => p.PermissionId == rolePermission.PermissionId);

        if (permission == null)
        {
            throw new Exception("Permission not found, verify the data sent");
        }

        var rolePermissionExists = await _context.RolePermissions
            .AnyAsync(rp => rp.RoleId == rolePermission.RoleId && rp.PermissionId == rolePermission.PermissionId);

        if (rolePermissionExists)
        {
            throw new Exception($"Role '{role.Name}' already has permission '{permission.Name}'.");
        }

        var newRolePermission = new RolePermission
        {
            RoleId = role.RoleId,
            PermissionId = permission.PermissionId
        };

        _context.RolePermissions.Add(newRolePermission);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRolePermission(CreateRolePermissionRequest rolePermissionRequest){
        var rolePermission = await _context.RolePermissions.FindAsync(rolePermissionRequest.RoleId, rolePermissionRequest.PermissionId);
        if(rolePermission == null){
            throw new Exception("Role permission not found, verify the data sent");
        }
        _context.RolePermissions.Remove(rolePermission);
        await _context.SaveChangesAsync();
    }
}