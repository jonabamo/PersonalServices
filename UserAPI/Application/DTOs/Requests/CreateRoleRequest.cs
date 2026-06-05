namespace UserAPI.Application.DTOs.Requests
{
    public class CreateRoleRequest
    {
        public int RoleId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}