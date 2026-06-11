using UserAPI.Domain.Entities;
using UserAPI.Application.Interfaces;
using UserAPI.Data;
using Microsoft.EntityFrameworkCore;
using UserAPI.Application.DTOs;

namespace UserAPI.Application.Services;
public class PermissionService : IPermissionService{
    
    private readonly AppDbContext _context;

    public PermissionService(AppDbContext context){
        _context = context;
    }

    public async Task<IEnumerable<GetPermisionResponse>> GetAllPermissions(){
        return await _context.Permissions.Select(p => GetPermisionResponse.ToResponse(p.PermissionId, p.Name)).ToListAsync();
    }

    public async Task<GetPermisionResponse?> GetPermissionById(int id){
        var permission = await _context.Permissions.FirstOrDefaultAsync(p => p.PermissionId == id);
        return permission == null 
        ? GetPermisionResponse.ToNotFoundResponse()
        : GetPermisionResponse.ToResponse(permission.PermissionId, permission.Name);
    }

    public async Task<GetPermisionResponse> GetPermissionByName(string name){
        var permission = await _context.Permissions.FirstOrDefaultAsync(p => p.Name == name);
        return permission == null 
        ? GetPermisionResponse.ToNotFoundResponse() 
        : GetPermisionResponse.ToResponse(permission.PermissionId, permission.Name);
    }
    public async Task<int> CreatePermission(CreatePermissionRequest request){
        var existingPermission = await _context.Permissions.FirstOrDefaultAsync(p => p.Name == request.Name);
        if(existingPermission != null) return existingPermission.PermissionId;
        var newPermission = new Permission{ Name = request.Name};
        _context.Permissions.Add(newPermission);
        await _context.SaveChangesAsync();
        return newPermission.PermissionId;
    }

    public async Task UpdatePermissionById(int id, CreatePermissionRequest request){
        var existingPermission = await _context.Permissions.FirstOrDefaultAsync(p => p.PermissionId == id);
        if(existingPermission == null) throw new Exception("Permission not found");
        existingPermission.Name = request.Name;
        _context.Permissions.Update(existingPermission);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePermissionById(int id){
        var existingPermission = await _context.Permissions.FirstOrDefaultAsync(p => p.PermissionId == id);
        if(existingPermission == null) throw new Exception("Permission not found");
        _context.Permissions.Remove(existingPermission);
        await _context.SaveChangesAsync();
    }
}