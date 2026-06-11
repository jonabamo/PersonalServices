using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Application.DTOs;
using UserAPI.Application.Interfaces;

namespace UserAPI.Controllers;

[ApiController]
[Route("api/user")] 
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILoginService _loginService;

    // 200 OK
    // 201 Created
    // 400 Bad Request
    // 401 Unauthorized
    // 403 Forbidden
    // 404 Not Found
    // 409 Conflict
    // 500 Internal Server Error

    public UserController(IUserService userService, ILoginService loginService)
    {
        _userService = userService;
        _loginService = loginService;
    }

    [HttpPost("/login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        return Ok(await _loginService.Login(request));
    }

    [HttpPost("/create-user")]
    [Authorize(Policy = "CanCreate")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var result = await _userService.CreateUser(request);
        return result.Success 
        ? StatusCode(StatusCodes.Status201Created, result) 
        : StatusCode(result.StatusCode, result);
    }

    [HttpPost("/create-users")]
    [Authorize(Policy = "CanCreate")]
    public async Task<IActionResult> CreateUserBulk([FromBody] List<CreateUserRequest> request)
    {
        var results = await _userService.CreateUsers(request);
        return results.All(r => r.Success) 
        ? StatusCode(StatusCodes.Status201Created, results) 
        : StatusCode(results.FirstOrDefault()?.StatusCode ?? 400, results);
    }

    [HttpPut("/update-user/{id}")]
    [Authorize(Policy = "CanEdit")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
    {
        //return Ok(await _userService.UpdateUserById(id, request));
        var result = await _userService.UpdateUserById(id, request);
        return result.Success 
        ? StatusCode(StatusCodes.Status201Created, result) 
        : StatusCode(result.StatusCode, result);
    }

    [HttpDelete("/delete-user/{id}")]
    [Authorize(Policy = "CanDelete")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _userService.DeleteUserById(id);
        return result.Success 
        ? StatusCode(StatusCodes.Status201Created, result) 
        : StatusCode(result.StatusCode, result);
        
    }

    [HttpDelete("/delete-users")]
    [Authorize(Policy = "CanDelete")]
    public async Task<IActionResult> DeleteAll()
    {
        var result = await _userService.DeleteAllUsers();
        return result.Success
        ? StatusCode(StatusCodes.Status201Created, result) 
        : StatusCode(result.StatusCode, result);
    }

    [HttpGet("/get-all-users")]
    [Authorize(Policy = "CanView")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _userService.GetAllUsers());
    }

    [HttpGet("/count-users")]
    [Authorize(Policy = "CanView")]
    public async Task<long> GetCount()
    {
        return await _userService.GetUsersCountAsync();
    }

    [HttpGet("/get-users/{startsWith}")]
    [Authorize(Policy = "CanView")]
    public async Task<IActionResult> GetStartsWith(string startsWith)
    {
        return Ok(await _userService.GetUsersStartsWith(startsWith));
    }

    [HttpGet("/get-user-by-id/{id}")]
    [Authorize(Policy = "CanView")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _userService.GetUserById(id));
    }

}
