namespace TaskAPI.Domain.Entities;
public class Permission
{
    public int PermissionId { get; private set; }
    public string Name { get; set; } = string.Empty;

    public Permission(string name){
        Name = name;
    }

    public Permission(){
        Name = string.Empty;
    }

    public ICollection<RolePermission> RolePermissions { get; set; } = new HashSet<RolePermission>();
}