using System.Collections;
using Microsoft.EntityFrameworkCore;
using UserAPI.Application.DTOs;
using UserAPI.Application.Interfaces;
using UserAPI.Common;
using UserAPI.Data;
using UserAPI.Domain.Entities;

namespace UserAPI.Application.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IRoleService _roleService;

        public UserService(AppDbContext context, IConfiguration configuration, IRoleService roleService)
        {
            _context = context;
            _configuration = configuration;
            _roleService = roleService;
        }

        public async Task<long> GetUsersCountAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<BaseListResponse<UserResponseItemDto>> GetAllUsers()
        {
            var databaseUsers = await _context.Users.AsNoTracking().ToListAsync();

            var uniqueRoleNames = databaseUsers
            .Select(u => u.Role)
            .Where(name => !string.IsNullOrEmpty(name))
            .Distinct()
            .ToList();

            var rolesDictionary = await _context.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .Where(r => uniqueRoleNames.Contains(r.Name))
                .ToDictionaryAsync(
                    r => r.Name.ToLower(), 
                    r => GetRoleResponse.ToResponse(
                        r.RoleId,
                        r.Name, 
                        r.RolePermissions.Select(rp => rp.Permission.Name).ToList()
                    )
                );

            var usersList = databaseUsers.Select(u => 
            {
                string userRoleKey = u.Role?.ToLower() ?? string.Empty;
                rolesDictionary.TryGetValue(userRoleKey, out var roleData);

                return new UserResponseItemDto
                {
                    User = new UserDto
                    {
                        Name = u.Name,
                        Email = u.Email
                    },
                    Role = new RoleDto
                    {
                        RoleId = roleData?.RoleId ?? 0,
                        RoleName = u.Role ?? "Unknown",
                        Permissions = roleData?.Permissions ?? new List<string>()
                    }
                };
            }).ToList();

            var response = new BaseListResponse<UserResponseItemDto>
            {
                Success = true,
                Message = usersList.Count > 0 ? "Users Found" : "No users found",
                StatusCode = 200,
                TotalRecords = usersList.Count,
                Data = usersList
            };

            return response;
        }

        public async Task<string> GetUserPassword(string userEmail)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(userEmail));
            if (user == null) throw new Exception("Error getting user's password!");
            return user.Password;
        }

        public async Task<BaseListResponse<UserResponseItemDto>> GetUsersStartsWith(string startsWith)
        {
            var databaseUsers = await _context.Users.AsNoTracking().Where(u => u.Name.StartsWith(startsWith)).ToListAsync();

            var uniqueRoleNames = databaseUsers
            .Select(u => u.Role)
            .Where(name => !string.IsNullOrEmpty(name))
            .Distinct()
            .ToList();

            var rolesDictionary = await _context.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .Where(r => uniqueRoleNames.Contains(r.Name))
                .ToDictionaryAsync(
                    r => r.Name.ToLower(), 
                    r => GetRoleResponse.ToResponse(
                        r.RoleId,
                        r.Name, 
                        r.RolePermissions.Select(rp => rp.Permission.Name).ToList()
                    )
                );

            var usersList = databaseUsers.Select(u => 
            {
                string userRoleKey = u.Role?.ToLower() ?? string.Empty;
                rolesDictionary.TryGetValue(userRoleKey, out var roleData);

                return new UserResponseItemDto
                {
                    User = new UserDto
                    {
                        Name = u.Name,
                        Email = u.Email
                    },
                    Role = new RoleDto
                    {
                        RoleId = roleData?.RoleId ?? 0,
                        RoleName = u.Role ?? "Unknown",
                        Permissions = roleData?.Permissions ?? new List<string>()
                    }
                };
            }).ToList();

            var response = new BaseListResponse<UserResponseItemDto>
            {
                Success = true,
                Message = usersList.Count > 0 ? "Users Found" : "No users found",
                StatusCode = 200,
                TotalRecords = usersList.Count,
                Data = usersList
            };

            return response;
        }

        public async Task<CreateUserResponse> CreateUser(CreateUserRequest request)
        {
            try
            {
                var user = await _context.Users.AsNoTracking().Where(u => u.Email.Equals(request.Email)).FirstOrDefaultAsync();

                if (user == null)
                {
                    user = new User(request.Name, request.Email, request.Role, Utils.SetPassword(request.Password));
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                }
                
                var rolePermissions = await _roleService.GetAllRolePermissions(request.Role);
                return CreateUserResponse.ToResponse(user, rolePermissions);
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex);
                return CreateUserResponse.ToErrorResponse();
            }
        }

        public async Task<List<CreateUserResponse>> CreateUsers(List<CreateUserRequest> incomingUsers)
        {
            try
            {
                var incomingEmails = incomingUsers.Select(u => u.Email).ToList();

                var existingEmailsSet = (await _context.Users
                    .Where(u => incomingEmails.Contains(u.Email))
                    .Select(u => u.Email.ToLower())
                    .ToListAsync())
                    .ToHashSet();

                var usersToCreateDtos = incomingUsers
                    .Where(u => !existingEmailsSet.Contains(u.Email.ToLower()))
                    .ToList();

                if (usersToCreateDtos.Any())
                {
                    var newUsersEntities = usersToCreateDtos.Select(dto => new User (
                        dto.Name,
                        dto.Email,
                        dto.Role,
                        Utils.SetPassword(dto.Password) 
                    )).ToList();

                    await _context.Users.AddRangeAsync(newUsersEntities);
                    await _context.SaveChangesAsync();
                }

                var users = incomingUsers.Select(user => new User(user.Name, user.Email, user.Role, Utils.SetPassword(user.Password))).ToList();

                var responses = new List<CreateUserResponse>();

                for (int i = 0; i < users.Count; i++)
                {
                    var rolePermissions = await _roleService.GetAllRolePermissions(incomingUsers[i].Role);
                    responses.Add(CreateUserResponse.ToResponse(users[i], rolePermissions));
                }

                return responses;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex);
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
            if (user == null) return GetUserResponse.ToNotFoundResponse();
            var rolePermissions = await _roleService.GetAllRolePermissions(user.Role);
            return GetUserResponse.ToResponse(user, rolePermissions);
        }
    }
}