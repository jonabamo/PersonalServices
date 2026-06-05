namespace TaskAPI.Application.DTOs.Responses
{
    public class GetPermisionResponse
    {
        public int PermissionId { get; set; }
        public string Name { get; set; } = string.Empty;

        public GetPermisionResponse(int permissionId, string name)
        {
            PermissionId = permissionId;
            Name = name;
        }
        
        public static GetPermisionResponse ToNotFoundResponse(string message) => 
            new GetPermisionResponse(0, message);
    }
}
