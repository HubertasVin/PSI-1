using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Repository;

namespace Project.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly ILogger<CommentController> _logger;
    
    public CommentController(ICommentRepository commentRepository, ILogger<CommentController> logger)
    {
        _logger = logger;
        _commentRepository = commentRepository;
    }
    
    [HttpGet("get/{topicId}")]
    public IActionResult GetComments(string topicId)
    {
        try
        {
            List<Comment> comments = _commentRepository.GetAllComments(topicId);
            return Ok(comments);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting comments");
        }
        return BadRequest("Error getting comments");
    }
    [HttpGet("getComment/{commentId}")]
    public IActionResult GetCommentWithId(string commentId)
    {
        try
        {
            Comment? comment = _commentRepository.GetCommentById(commentId);
            return Ok(comment);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting comments");
        }
        return BadRequest("Error getting comments");
    }
    
    [HttpPost("add")]
    public IActionResult AddComment([FromBody] Comment comment)
    {
        try
        {
            bool result = _commentRepository.AddComment(comment);
            if (result)
            {
                return Ok("Comment added successfully");
            }
            
            return BadRequest("Failed to add comment");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error adding comment");
            return BadRequest("Error adding comment");
        }
    }

}