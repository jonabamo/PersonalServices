namespace TaskAPI.Application.Interfaces;

using Microsoft.AspNetCore.Identity.Data;
using TaskAPI.Application.DTOs.Requests;
using TaskAPI.Application.DTOs.Responses;

public interface ILoginService
{
    Task<AuthResponse> Login(LoginRequest request);
}
