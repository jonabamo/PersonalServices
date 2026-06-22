using UserAPI.Application.Interfaces;

namespace UserAPI.Application.DTOs
{
    public class AuthResponse : IServiceResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }
        public int StatusCode { get; set; } = 200;

        public static AuthResponse ToSuccessResponse(string token)
            => new() { Success = true, Message = "Login successful", Token = token, StatusCode = 200 };

        public static AuthResponse ToErrorResponse()
            => new() { Success = false, Message = "Invalid credentials", StatusCode = 401 };
        
        public static AuthResponse ToErrorPasswordResponse()
            => new() { Success = false, Message = "Invalid password", StatusCode = 401 };

        public static AuthResponse ToDeletedAccountResponse()
            => new() { Success = false, Message = "User account has been deleted", StatusCode = 403 };

        public static AuthResponse ToErrorResponse(string message, int statusCode)
            => new() { Success = false, Message = message, StatusCode = statusCode };
    }
}