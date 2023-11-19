using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Project.Contents;
using Project.Models;

namespace Project.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserContents _userContents;

    public UserController(UserContents userContents)
    {
        _userContents = userContents;
    }
    
    [HttpGet("get/{id}")]
    public IActionResult GetUserName(string id)
    {
        return Ok(_userContents.Get(id));
    }
    
    // [HttpGet("list/{subjectId}")]
    // public IActionResult ListTopics(string subjectId)
    // {
    //     return Ok(_userContents.GetUserList(subjectId));
    // }
    
    [HttpGet("list")]
    public IActionResult ListUsers()
    {
        return Ok(_userContents.GetUserList());
    }

    [HttpPost("register")]
    public IActionResult UploadTopic([FromBody] JsonElement request)
    {
        string? userEmail = request.GetProperty("userEmail").GetString();
        string? userPassword = request.GetProperty("userPassword").GetString();
        
        if (userEmail == null && userPassword == null)
            return BadRequest("Email and password fields cannot be empty");
        if (userEmail == null)
            return BadRequest("Email field cannot be empty");
        if (userPassword == null)
            return BadRequest("Password field cannot be empty");

        if (!_userContents.IsEmailTaken(userEmail))
            return BadRequest("Email already exists");
        if (!UserContents.IsEmailValid(userEmail))
            return BadRequest("Email is not valid");
        
        User? addedUser = _userContents.CreateUser(request);
        return addedUser == null
            ? BadRequest("Invalid register credentials")
            : Ok(addedUser);
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] JsonElement request)
    {
        string? userId = _userContents.CheckLogin(request);

        return userId == null
            ? BadRequest("Invalid login credentials")
            : Ok(userId);
    }

    [HttpPost("isEmailTaken")]
    public IActionResult isEmailTaken([FromBody] JsonElement request)
    {
        string? userEmail = request.GetProperty("userEmail").GetString();
        if (userEmail == null)
        {
            return BadRequest("Invalid request body");
        }

        if (!_userContents.IsEmailTaken(userEmail))
        {
            return BadRequest("Email already exists");
        }

        return Ok(userEmail);
    }
}