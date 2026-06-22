namespace UserAPI.Domain.Entities;
public class Role
{
    public int RoleId { get; private set; }
    public string Name { get; set; } = string.Empty;

    public Role(string name){
        Name = name;
    }

    public Role(){
        Name = string.Empty;
    }

    public ICollection<RolePermission> RolePermissions { get; set; } = new HashSet<RolePermission>();
}