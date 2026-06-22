namespace UserAPI.Application.Interfaces;

using Microsoft.AspNetCore.Identity.Data;
using UserAPI.Application.DTOs;

public interface ILoginService
{
    Task<AuthResponse> Login(LoginRequest request);
}
