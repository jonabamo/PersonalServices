namespace UserAPI.Application.Interfaces;

using Microsoft.AspNetCore.Identity.Data;
using UserAPI.Application.DTOs.Requests;
using UserAPI.Application.DTOs.Responses;

public interface ILoginService
{
    Task<AuthResponse> Login(LoginRequest request);
}
