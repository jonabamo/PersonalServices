using System.Numerics;

public class PermissionRoleDto
{
    public int RoleId { get; set; } = 0;
    public string ?RoleName { get; set; } = string.Empty;
    // public int PermissionId { get; set; } = 0;
    // public string ?PermissionName { get; set; } = string.Empty;
    //public Dictionary<string, List<string>> ?PermissionRoles { get; set;} = null;
    public List<string> ?Permissions { get; set;} = null;
}