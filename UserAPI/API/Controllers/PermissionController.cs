using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Application.DTOs.Requests;
using UserAPI.Application.Interfaces;
using UserAPI.Application.Services;

namespace UserAPI.API.Controllers
{
    [ApiController]
    [Route("api/permission")]
    public class PermissionController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;
        private readonly IRolePermissionService _rolePermissionService;

        public PermissionController(IRoleService roleService, IPermissionService permissionService, IRolePermissionService rolePermissionService)
        {
            _roleService = roleService;
            _permissionService = permissionService;
            _rolePermissionService = rolePermissionService;
        }

        [HttpGet("/get-all-role-permissions")]
        [Authorize(Policy = "CanView")]
        public async Task<IActionResult> GetAllRolePermission()
        {
            return Ok(await _rolePermissionService.GetAllRolePermissions());
        }

        [HttpGet("/get-role-permissions/{roleName}")]
        [Authorize(Policy = "CanView")]
        public async Task<IActionResult> GetRolePermissionByRoleName(string roleName)
        {
            return Ok(await _rolePermissionService.GetRolePermissionByName(roleName));
        }

        [HttpPost("/create-role-permission")]
        [Authorize(Policy = "CanCreate")]
        public async Task<IActionResult> CreateRolePermission([FromBody] CreateRolePermissionRequest request)
        {
            return Ok(await _rolePermissionService.CreateRolePermission(request.RoleId, request.PermissionId));
        }
    }
}
