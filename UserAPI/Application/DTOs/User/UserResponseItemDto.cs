public class UserResponseItemDto
{
    // public bool Success { get; set; } = true;
    // public string Message { get; set; } = "User found";
    // public int StatusCode { get; set; } = 200;
    public UserDto User { get; set; } = new();
    public RoleDto Role { get; set; } = new();
}