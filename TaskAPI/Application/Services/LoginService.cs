using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskAPI.Application.DTOs.Requests;
using TaskAPI.Application.DTOs.Responses;
using TaskAPI.Application.Interfaces;
using TaskAPI.Data;
using TaskAPI.Configurations;
using TaskAPI.Common;

namespace TaskAPI.Application.Services
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

            // Asignar permisos basados en el rol para que las Policies funcionen
            // claims.Add(new Claim("Permission", AppPermissions.ViewData)); 
            
            // if (role == AppRoles.Admin || role == AppRoles.Manager || role == AppRoles.SuperAdmin) {
            //     claims.Add(new Claim("Permission", AppPermissions.CreateData));
            //     claims.Add(new Claim("Permission", AppPermissions.EditData));
            // }
            // if (role == AppRoles.Manager || role == AppRoles.SuperAdmin) {
            //     claims.Add(new Claim("Permission", AppPermissions.DeleteData));
            // }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null) return AuthResponse.ToErrorResponse();
            if (Utils.ValidPassword(request.Password, user.Password) == false) return AuthResponse.ToErrorPasswordResponse();
            var token = GenerateToken(user.Id, user.Email, user.Role);
            return AuthResponse.ToSuccessResponse(token);
        }
    }
}
