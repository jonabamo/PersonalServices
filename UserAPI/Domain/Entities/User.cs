using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using UserAPI.Common;
using UserAPI.Application.DTOs.Requests;
using UserAPI.Application.DTOs.Responses;
using UserAPI.Application.Interfaces;
using UserAPI.Data;
using Microsoft.EntityFrameworkCore;


namespace UserAPI.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    [MaxLength(50)]
    public string Name { get; private set; } = string.Empty;
    [MaxLength(120)]
    public string Email { get; private set; } = string.Empty;
    [MaxLength(20)]
    public string Role { get; private set; } = "User";
    public string Password { get; private set; } = string.Empty;

    public User(string name, string email, string role, string password){
        if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty or default!", nameof(name));
        if(string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be empty or default!", nameof(email));
        if(string.IsNullOrWhiteSpace(role)) throw new ArgumentException("Role cannot be empty or default!", nameof(role));
        if(string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password cannot be empty or default!", nameof(password));

        if(name.Equals("string") || email.Equals("user@example.com"))
            throw new ArgumentException("Invalid name or email!");
            
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Role = role;
        Password = password;
    }

    public void Update(string name, string email, string role, string password){
        if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty or default!", nameof(name));
        if(string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be empty or default!", nameof(email));
        if(string.IsNullOrWhiteSpace(role)) throw new ArgumentException("Role cannot be empty or default!", nameof(role));

        if(name.Equals("string") || email.Equals("user@example.com"))
            throw new ArgumentException("Invalid name or email!");
        
        Name = name;
        Email = email;
        Role = role;
        Password = password;
    }
 
}
