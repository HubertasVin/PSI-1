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
        try {
            var user = _userContents.Get(id);
            return Ok(user?.Name);
        }
        catch (UserNotFoundException e) {
            Console.WriteLine(e);
            LogToFile.LogException(e);
            return BadRequest(e.Message);
        }
    }
    
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
        try {
            return Ok(_userContents.CheckLogin(request));
        }
        catch (UserLoginRegisterException e) {
            Console.WriteLine(e);
            LogToFile.LogException(e);
            return BadRequest(e.Message);
        }
    }
}