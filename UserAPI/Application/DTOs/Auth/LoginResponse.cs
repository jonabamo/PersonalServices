using UserAPI.Application.Interfaces;

namespace UserAPI.Application.DTOs
{
    public class AuthResponse : IServiceResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }

        public static AuthResponse ToSuccessResponse(string token)
            => new() { Success = true, Message = "Login successful", Token = token };

        public static AuthResponse ToErrorResponse()
            => new() { Success = false, Message = "Invalid credentials" };
        public static AuthResponse ToErrorPasswordResponse()
            => new() { Success = false, Message = "Invalid password" };
    }
}