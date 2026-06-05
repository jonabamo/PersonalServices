using Azure.Core;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskAPI.Application.DTOs.Requests;
using TaskAPI.Application.DTOs.Responses;
using TaskAPI.Application.Interfaces;
using TaskAPI.Common;
using TaskAPI.Data;
using TaskAPI.Domain.Entities;

namespace TaskAPI.Application.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<long> GetUsersCountAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<List<GetUserResponse>> GetAllUsers()
        {
            var users = await _context.Users.AsNoTracking().ToListAsync();
            return users.Select(user => GetUserResponse.ToResponse(user)).ToList();
        }

        public async Task<string> GetUserPassword(string userEmail)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(userEmail));
            if (user == null) throw new Exception("Error getting user's password!");
            return user.Password;
        }

        public async Task<List<GetUserResponse>> GetUsersStartsWith(string startsWith)
        {
            var users = await _context.Users.AsNoTracking().Where(u => u.Name.StartsWith(startsWith)).ToListAsync();
            return users.Select(user => GetUserResponse.ToResponse(user)).ToList();
        }

        public async Task<CreateUserResponse> CreateUser(CreateUserRequest request)
        {
            try
            {
                var user = new User(request.Name, request.Email, request.Role, Utils.SetPassword(request.Password));
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return CreateUserResponse.ToSuccessResponse(user.Name, user.Email);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException)
                {
                    switch (sqlException.Number)
                    {
                        case 8152 or 2628: // String or binary would be truncated
                            return request.Name.Length > 50 ? CreateUserResponse.ToTruncateResponse("Name") : CreateUserResponse.ToTruncateResponse("Email");
                        case 2601: // Unique index constraint violation
                            return CreateUserResponse.ToDuplicateResponse();
                        case 547: // Foreign key constraint violation
                            return CreateUserResponse.ToErrorResponse();
                        case 2627: // Unique constraint violation
                            return CreateUserResponse.ToErrorResponse();
                        default:
                            return CreateUserResponse.ToErrorResponse();
                    }
                }
                return CreateUserResponse.ToErrorResponse();
            }
        }

        public async Task<List<CreateUserResponse>> CreateUsers(List<CreateUserRequest> requests)
        {
            try
            {
                var users = requests.Select(user => new User(user.Name, user.Email, user.Role, Utils.SetPassword(user.Password))).ToList();
                await _context.Users.AddRangeAsync(users);
                await _context.SaveChangesAsync();
                return requests.Select(user => CreateUserResponse.ToSuccessResponse(user.Name, user.Email)).ToList();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException)
                {
                    if (sqlException.Number == 8152 || sqlException.Number == 2628)
                    {
                        var results = new List<CreateUserResponse>();
                        foreach (var request in requests)
                        {
                            if (request.Name.Length > 50) results.Add(CreateUserResponse.ToTruncateResponse("Name"));
                            else if (request.Email.Length > 50) results.Add(CreateUserResponse.ToTruncateResponse("Email"));
                            else results.Add(CreateUserResponse.ToErrorResponse());
                        }
                        return results;
                    }
                }
                return new List<CreateUserResponse> { CreateUserResponse.ToErrorResponse() };
            }
        }

        public async Task<UpdateUserResponse> UpdateUserById(Guid id, UpdateUserRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return UpdateUserResponse.ToNotFoundUpdateResponse(request.Name);
            user.Update(request.Name, request.Email, request.Role, Utils.SetPassword(request.Password));
            await _context.SaveChangesAsync();
            return UpdateUserResponse.ToUpdateResponse();
        }

        public async Task<DeleteUserResponse> DeleteUserById(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return DeleteUserResponse.ToNotFoundDeleteResponse(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return DeleteUserResponse.ToSuccessDeleteResponse();
        }

        public async Task<DeleteUserResponse> DeleteAllUsers()
        {
            _context.Users.RemoveRange(_context.Users);
            await _context.SaveChangesAsync();
            return DeleteUserResponse.ToSuccessDeleteAllResponse();
        }

        public async Task<GetUserResponse> GetUserById(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return GetUserResponse.ToNotFoundUpdateResponse();
            return GetUserResponse.ToResponse(user);
        }
    }
}