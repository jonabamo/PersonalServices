using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserAPI.Application.DTOs;
using UserAPI.Application.Services;
using UserAPI.Data;
using UserAPI.Common;

namespace UserAPI.Tests.Services;

public class UserServiceTests
{
    [Fact]
    public async Task CreateUser_Should_Create_New_User()
    {
        // Arrange

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);

        var configuration = new ConfigurationBuilder().Build();

        var service = new UserService(context, configuration);

        var request = new CreateUserRequest
        {
            Name = "Jonathan",
            Email = "jonathan@test.com",
            Password = "Password123",
            Role = "Admin"
        };

        // Act

        var response = await service.CreateUser(request);

        // Assert

        response.Should().NotBeNull();

        context.Users.Count().Should().Be(1);

        var user = await context.Users.FirstAsync();

        user.Name.Should().Be("Jonathan");

        user.Email.Should().Be("jonathan@test.com");
    }
    
    [Fact]
    public async Task CreateUser_Should_Hash_Password()
    {
        // Arrange

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);

        var configuration = new ConfigurationBuilder().Build();

        var service = new UserService(context, configuration);

        var request = new CreateUserRequest
        {
            Name = "Jonathan",
            Email = "jonathan@test.com",
            Password = "Password123",
            Role = "Admin"
        };

        // Act

        var response = await service.CreateUser(request);

        // Assert

        response.Should().NotBeNull();

        context.Users.Count().Should().Be(1);

        var user = await context.Users.FirstAsync();

        user.Password.Should().NotBe("Password123");

        Utils.ValidPassword(
            "Password123",
            user.Password)
            .Should()
            .BeTrue();

        Utils.ValidPassword(
            "WrongPassword",
            user.Password)
            .Should()
            .BeFalse();
    }

    [Fact]
    public async Task GetUserById_Should_Return_NotFound_When_User_Does_Not_Exist()
    {
        // Arrange

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);

        var configuration = new ConfigurationBuilder().Build();

        var service = new UserService(context, configuration);

        var nonExistingId = Guid.NewGuid();

        // Act

        var response = await service.GetUserById(nonExistingId);

        // Assert

        response.Should().NotBeNull();

        response.Success.Should().BeFalse();

        response.Message.Should().Be(ErrorCodes.UserNotFound);

        response.Name.Should().BeEmpty();

        response.Role.Should().BeEmpty();
    }
}