using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Project.Exceptions;
using Project.Models;
using Project.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace Project.Controllers;

[Authorize]
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

    [HttpGet("get-notes/{userId}")]
    public IActionResult GetUserNotes(string userId)
    {
        try
        {
            // Retrieve and return the private notes for the user
            List<string> userNotes = _userRepository.GetUserNotes(userId);
            return Ok(userNotes);
        }
        catch (UserNotFoundException e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpPost("save-note/{userId}")]
    public IActionResult SaveUserNote(string userId, [FromBody] string note)
    {
        try
        {
            // Save the private note for the user
            _userRepository.SaveUserNote(userId, note);
            return Ok("Note saved successfully");
        }
        catch (UserNotFoundException e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }
}