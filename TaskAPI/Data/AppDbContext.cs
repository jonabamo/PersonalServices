namespace TaskAPI.Data;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore;
using TaskAPI.Application.Interfaces;
using TaskAPI.Application.Services;
using TaskAPI.Common;
using TaskAPI.Domain.Entities;
using TaskAPI.Application.DTOs.Requests;

public class AppDbContext : DbContext{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder){

        //Composite PK
        modelBuilder.Entity<RolePermission>().HasKey(rp => new { rp.RoleId, rp.PermissionId });

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId);

        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<User>().HasIndex(u => new {u.Name, u.Email}).IsUnique();
        modelBuilder.Entity<User>().Property(u => u.Name).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<User>().Property(u => u.Email).HasMaxLength(120).IsRequired();
        modelBuilder.Entity<User>().Property(u => u.Role).HasMaxLength(20).IsRequired();
    }
}

public static class AppDbContextExtensions
{

    public static async Task SeedInitialData(this AppDbContext context, IUserService userService, IRoleService roleService, IPermissionService permissionService, IRolePermissionService rolePermissionService)
    {
        await context.Database.EnsureCreatedAsync();
        if (await context.Users.AnyAsync()) return; 

        await roleService.CreateRole(new CreateRoleRequest { Name = "User" });
        await roleService.CreateRole(new CreateRoleRequest { Name = "Manager" });
        await roleService.CreateRole(new CreateRoleRequest { Name = "Admin" });
        await roleService.CreateRole(new CreateRoleRequest { Name = "SuperAdmin" });

        await permissionService.CreatePermission(new CreatePermissionRequest { Name = "ViewData" });
        await permissionService.CreatePermission(new CreatePermissionRequest { Name = "CreateData" });
        await permissionService.CreatePermission(new CreatePermissionRequest { Name = "EditData" });
        await permissionService.CreatePermission(new CreatePermissionRequest { Name = "DeleteData" });

        await rolePermissionService.CreateRolePermission("User", "ViewData");

        await rolePermissionService.CreateRolePermission("Manager", "ViewData");
        await rolePermissionService.CreateRolePermission("Manager", "CreateData");
        await rolePermissionService.CreateRolePermission("Manager", "EditData");
        
        await rolePermissionService.CreateRolePermission("Admin", "ViewData");
        await rolePermissionService.CreateRolePermission("Admin", "CreateData");
        await rolePermissionService.CreateRolePermission("Admin", "EditData");

        await rolePermissionService.CreateRolePermission("SuperAdmin", "ViewData");
        await rolePermissionService.CreateRolePermission("SuperAdmin", "CreateData");
        await rolePermissionService.CreateRolePermission("SuperAdmin", "EditData");
        await rolePermissionService.CreateRolePermission("SuperAdmin", "DeleteData");

        await userService.CreateUser(new Application.DTOs.Requests.CreateUserRequest { Name = "John Doe (U)", Email = "johndoe@email.com", Role = "User", Password ="johndoe2026." });
        await userService.CreateUser(new Application.DTOs.Requests.CreateUserRequest { Name = "Jane Doe (M)", Email = "janedoe@email.com", Role = "Manager", Password = "janedoe2026." });
        await userService.CreateUser(new Application.DTOs.Requests.CreateUserRequest { Name = "Peter Pan (A)", Email = "peterpan@email.com", Role = "Admin", Password = "peterpan2026." });
        await userService.CreateUser(new Application.DTOs.Requests.CreateUserRequest { Name = "Helen Keller (SA)", Email = "hellenkeller@email.com", Role = "SuperAdmin", Password = "hellenkeller2026." });
    }
}