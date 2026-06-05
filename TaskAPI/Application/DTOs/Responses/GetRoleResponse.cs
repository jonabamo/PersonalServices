namespace TaskAPI.Application.DTOs.Responses;

public class GetRoleResponse
{
    public int RoleId { get; set; }
    public string Name { get; set; }
    public List<string> Permissions { get; set; }

    public GetRoleResponse(int roleId, string name, List<string> permissions) {
        RoleId = roleId;
        Name = name;
        Permissions = permissions;
    }

    public static GetRoleResponse ToNotFoundResponse(string message) => 
        new GetRoleResponse(0, message, new List<string>());
}
