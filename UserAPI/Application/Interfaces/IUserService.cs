namespace UserAPI.Application.Interfaces;

using UserAPI.Application.DTOs.Requests;
using UserAPI.Application.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.Data;

public interface IUserService
{
    Task<List<GetUserResponse>> GetAllUsers();
    Task<string> GetUserPassword(string userEmail);
    Task<List<GetUserResponse>> GetUsersStartsWith(string startsWith);
    Task<CreateUserResponse> CreateUser(CreateUserRequest request);
    Task<List<CreateUserResponse>> CreateUsers(List<CreateUserRequest> requests);
    Task<UpdateUserResponse> UpdateUserById(Guid id, UpdateUserRequest request);
    Task<DeleteUserResponse> DeleteUserById(Guid id);
    Task<DeleteUserResponse> DeleteAllUsers();
    Task<GetUserResponse> GetUserById(Guid id);
    Task<long> GetUsersCountAsync();
}
