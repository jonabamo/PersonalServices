public abstract class BaseResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public UserDto? User { get; set; }
    public PermissionRoleDto? Role { get; set; }
}