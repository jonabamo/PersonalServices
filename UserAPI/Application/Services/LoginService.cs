using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserAPI.Application.DTOs;
using UserAPI.Application.Interfaces;
using UserAPI.Data;
using UserAPI.Configurations;
using UserAPI.Common;

namespace UserAPI.Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public LoginService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private string GenerateToken(Guid userId, string email, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            if(RolePermissionMatrix.Roles.TryGetValue(role, out var permissions))
            {
                foreach(var permission in permissions)
                {
                    claims.Add(new Claim("Permission", permission));
                }
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AuthResponse> Login(LoginRequest request)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (user == null) return AuthResponse.ToErrorResponse();
                
                // Check if user is soft-deleted
                if (user.IsDeleted)
                {
                    return AuthResponse.ToDeletedAccountResponse();
                }
                
                if (Utils.ValidPassword(request.Password, user.Password) == false) return AuthResponse.ToErrorPasswordResponse();
                var token = GenerateToken(user.Id, user.Email, user.Role);
                return AuthResponse.ToSuccessResponse(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return AuthResponse.ToErrorResponse("Error during login", 500);
            }
        }
    }
}
