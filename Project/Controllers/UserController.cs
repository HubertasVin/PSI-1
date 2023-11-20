using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Repository;

namespace Project.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserRepository _userRepository;
    private readonly ILogger<UserController> _logger;

    public UserController(UserRepository userRepository, ILogger<UserController> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }
    
    [HttpGet("get/{id}")]
    public IActionResult GetUserName(string id)
    {
        try {
            return Ok(_userRepository.Get(id));
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
            return Ok(_userRepository.GetUserByEmail(email));
        }
        catch (UserNotFoundException e) {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("list")]
    public IActionResult ListUsers()
    {
        return Ok(_userRepository.GetUserList());
    }

    [HttpPost("register")]
    public IActionResult UploadTopic([FromBody] JsonElement request)
    {
        try {
            return Ok(_userRepository.CreateUser(request));
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
            return Ok(_userRepository.CheckLogin(request));
        }
        catch (UserLoginRegisterException e) {
            Console.WriteLine(e);
            LogToFile.LogException(e);
            return BadRequest(e.Message);
        }
    }
}