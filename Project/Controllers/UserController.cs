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
        Console.WriteLine("In upload topic");
        // Console.WriteLine(request);
        User? addedUser = _userContents.CreateUser(request);
        return addedUser == null
            ? BadRequest("Invalid request body")
            : Ok(addedUser);
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] JsonElement request)
    {
        string? userId = _userContents.CheckLogin(request);

        return userId == null
            ? BadRequest("Invalid request body")
            : Ok(userId);
    }

    [HttpPost("checkEmail")]
    public IActionResult CheckEmail([FromBody] JsonElement request)
    {
        if (!_userContents.CheckEmail(request.GetProperty("userEmail").GetString()))
        {
            return BadRequest("Email already exists");
        }
        string? userEmail = request.GetProperty("userEmail").GetString();

        return userEmail == null
            ? BadRequest("Invalid request body")
            : Ok(userEmail);
    }
}