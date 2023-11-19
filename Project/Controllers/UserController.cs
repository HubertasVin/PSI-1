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
        try {
            return Ok(_userContents.CreateUser(request));
        }
        catch (UserLoginRegisterException e) {
            Console.WriteLine(e);
            LogToFile.LogException(e);
            return BadRequest(e.Message);
        }
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
    public IActionResult IsEmailTaken([FromBody] JsonElement request)
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