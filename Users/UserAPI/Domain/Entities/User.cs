using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using UserAPI.Common;
using UserAPI.Application.DTOs;
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

    /// <summary>
    /// Soft delete flag. When true, user is logically deleted but not physically removed.
    /// </summary>
    public bool IsDeleted { get; private set; } = false;

    /// <summary>
    /// Timestamp of last update, used for optimistic concurrency control
    /// </summary>
    [Timestamp]
    public byte[]? RowVersion { get; set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

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
        IsDeleted = false;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
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
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Soft delete this user. Sets IsDeleted to true without physically removing the record.
    /// </summary>
    public void SoftDelete()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }
 
}
