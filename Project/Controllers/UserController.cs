using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Project.Exceptions;
using Project.Models;
using Project.Repository;

namespace Project.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserRepository userRepository, ILogger<UserController> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }
    
    [HttpGet("get/{id}")]
    public IActionResult GetUserName(string id)
    {
        try {
            return Ok(_userRepository.GetUser(id));
        }
        catch (ObjectNotFoundException e) {
            _logger.LogError(e, e.Message);
            return NotFound(e.Message);
        }
    }
    
    [HttpGet("get-by-email/{email}")]
    public IActionResult GetUserByEmail(string email)
    {
        try {
            return Ok(_userRepository.GetUserByEmail(email));
        }
        catch (ObjectNotFoundException e) {
            _logger.LogError(e, e.Message);
            return NotFound(e.Message);
        }
    }
    
    [HttpGet("list")]
    public IActionResult ListUsers()
    {
        return Ok(_userRepository.GetUserList());
    }

    [HttpPost("register")]
    public IActionResult UploadTopic([FromBody] User newUser)
    {
        try {
            return Ok(_userRepository.CreateUser(newUser));
        }
        catch (UserLoginRegisterException e) {
            _logger.LogError("Error occured when creating a new user", e);
            return BadRequest(e.Message);
        }
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User existingUser)
    {
        try
        {
            return Ok(_userRepository.CheckLogin(existingUser));
        }
        catch (UserLoginRegisterException e) {
            _logger.LogError("Error occured when checking credentials", e);
            return BadRequest(e.Message);
        }
    }
}