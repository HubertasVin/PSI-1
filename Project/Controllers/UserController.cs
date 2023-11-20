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
    private readonly ILogger<UserController> _logger;

    public UserController(UserContents userContents, ILogger<UserController> logger)
    {
        _userContents = userContents;
        _logger = logger;
    }
    
    [HttpGet("get/{id}")]
    public IActionResult GetUserName(string id)
    {
        try {
            return Ok(_userContents.Get(id));
        }
        catch (UserNotFoundException e) {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("get-by-email/{email}")]
    public IActionResult GetUserByEmail(string email)
    {
        try {
            return Ok(_userContents.GetUserByEmail(email));
        }
        catch (UserNotFoundException e) {
            _logger.LogError(e, e.Message);
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