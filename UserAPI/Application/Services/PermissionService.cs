using UserAPI.Domain.Entities;
using UserAPI.Application.Interfaces;
using UserAPI.Data;
using Microsoft.EntityFrameworkCore;
using UserAPI.Application.DTOs.Requests;
using UserAPI.Application.DTOs.Responses;

namespace UserAPI.Application.Services;
public class PermissionService : IPermissionService{
    
    private readonly AppDbContext _context;

    public PermissionService(AppDbContext context){
        _context = context;
    }

    public async Task<IEnumerable<GetPermisionResponse>> GetAllPermissions(){
        return await _context.Permissions.Select(p => new GetPermisionResponse(p.PermissionId, p.Name)).ToListAsync();
    }

    public async Task<GetPermisionResponse> GetPermissionResponseById(int id){
        var permission = await _context.Permissions.FindAsync(id);
        return permission == null ? GetPermisionResponse.ToNotFoundResponse("Permission not found") : new GetPermisionResponse(permission.PermissionId, permission.Name);
    }

    public async Task<Permission?> GetPermissionById(int id){
        var permission = await _context.Permissions.FindAsync(id);
        if(permission == null) throw new Exception("Permission not found");
        return permission;
    }

    public async Task<GetPermisionResponse> GetPermissionResponseByName(string name){
        var permission = await _context.Permissions.FirstOrDefaultAsync(p => p.Name == name);
        return permission == null ? GetPermisionResponse.ToNotFoundResponse("Permission not found") : new GetPermisionResponse(permission.PermissionId, permission.Name);
    }

    public async Task<Permission?> GetPermissionByName(string name){
        return await _context.Permissions.FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task<int> CreatePermission(CreatePermissionRequest request){
        var existingPermission = await GetPermissionByName(request.Name);
        if(existingPermission != null) return existingPermission.PermissionId;
        var newPermission = new Permission{ Name = request.Name};
        _context.Permissions.Add(newPermission);
        await _context.SaveChangesAsync();
        return newPermission.PermissionId;
    }

    public async Task UpdatePermissionById(int id, CreatePermissionRequest request){
        var existingPermission = await GetPermissionById(id);
        if(existingPermission == null) throw new Exception("Permission not found");
        existingPermission.Name = request.Name;
        _context.Permissions.Update(existingPermission);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePermissionById(int id){
        var existingPermission = await GetPermissionById(id);
        if(existingPermission == null) throw new Exception("Permission not found");
        _context.Permissions.Remove(existingPermission);
        await _context.SaveChangesAsync();
    }
}