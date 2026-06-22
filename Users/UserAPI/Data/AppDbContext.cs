namespace UserAPI.Data;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore;
using UserAPI.Application.Interfaces;
using UserAPI.Application.Services;
using UserAPI.Common;
using UserAPI.Domain.Entities;
using UserAPI.Application.DTOs;

public class AppDbContext : DbContext{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<IdempotencyLog> IdempotencyLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder){

        // Global query filter for soft delete - automatically filter deleted users
        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);

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
        modelBuilder.Entity<User>().Property(u => u.RowVersion).IsRowVersion();

        // Configure IdempotencyLog
        modelBuilder.Entity<IdempotencyLog>().HasKey(il => il.IdempotencyKey);
        modelBuilder.Entity<IdempotencyLog>().Property(il => il.IdempotencyKey).HasMaxLength(256).IsRequired();
        modelBuilder.Entity<IdempotencyLog>().Property(il => il.ResponseBody).IsRequired();
        modelBuilder.Entity<IdempotencyLog>().Property(il => il.RequestHash).HasMaxLength(64).IsRequired(); // SHA256 = 64 hex chars
        modelBuilder.Entity<IdempotencyLog>().Property(il => il.HttpMethod).HasMaxLength(10).IsRequired();
        modelBuilder.Entity<IdempotencyLog>().Property(il => il.Endpoint).HasMaxLength(255).IsRequired();
        modelBuilder.Entity<IdempotencyLog>().HasIndex(il => il.ExpiresAt); // For cleanup queries
    }
}

public static class AppDbContextExtensions
{

    public static async Task SeedInitialData(this AppDbContext context, IConfiguration configuration, IUserService userService, IRoleService roleService, IPermissionService permissionService, IRolePermissionService rolePermissionService)
    {
        string? adminPassword = configuration["SeedData:AdminPassword"];

        if (string.IsNullOrEmpty(adminPassword))
        {
            throw new Exception("Critical Error: Admin password for seeding is not configured in User Secrets!");
        }

        if (!await context.Roles.AnyAsync())
        {
            var userRoleId = await roleService.CreateRole(new CreateRoleRequest { Name = "User" });
            var managerRoleId = await roleService.CreateRole(new CreateRoleRequest { Name = "Manager" });
            var adminRoleId = await roleService.CreateRole(new CreateRoleRequest { Name = "Admin" });
            var superAdminRoleId = await roleService.CreateRole(new CreateRoleRequest { Name = "SuperAdmin" });

            var viewDataPermissionId = await permissionService.CreatePermission(new CreatePermissionRequest { Name = "ViewData" });
            var createDataPermissionId = await permissionService.CreatePermission(new CreatePermissionRequest { Name = "CreateData" });
            var editDataPermissionId = await permissionService.CreatePermission(new CreatePermissionRequest { Name = "EditData" });
            var deleteDataPermissionId = await permissionService.CreatePermission(new CreatePermissionRequest { Name = "DeleteData" });

            await rolePermissionService.CreateRolePermission(userRoleId, viewDataPermissionId);

            await rolePermissionService.CreateRolePermission(managerRoleId, viewDataPermissionId);
            await rolePermissionService.CreateRolePermission(managerRoleId, createDataPermissionId);
            await rolePermissionService.CreateRolePermission(managerRoleId, editDataPermissionId);

            await rolePermissionService.CreateRolePermission(adminRoleId, viewDataPermissionId);
            await rolePermissionService.CreateRolePermission(adminRoleId, createDataPermissionId);
            await rolePermissionService.CreateRolePermission(adminRoleId, editDataPermissionId);

            await rolePermissionService.CreateRolePermission(superAdminRoleId, viewDataPermissionId);
            await rolePermissionService.CreateRolePermission(superAdminRoleId, createDataPermissionId);
            await rolePermissionService.CreateRolePermission(superAdminRoleId, editDataPermissionId);
            await rolePermissionService.CreateRolePermission(superAdminRoleId, deleteDataPermissionId);
        }

        if (!await context.Users.AnyAsync())
        {
            await userService.CreateUser(new Application.DTOs.CreateUserRequest
            {
                Name = "Helen Keller (SA)",
                Email = "hellenkeller@email.com",
                Role = "SuperAdmin",
                Password = adminPassword
            });
        }
    }
}