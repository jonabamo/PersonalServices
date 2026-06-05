namespace UserAPI.Application.DTOs.Requests;

public class CreateRolePermissionRequest
{
    public int RoleId { get; set; }
    public int PermissionId { get; set; }
}