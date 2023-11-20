using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Project.Contents;
using Project.Models;

namespace Project.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly CommentContents _commentContents;
    private readonly ILogger<CommentController> _logger;
    
    public CommentController(CommentContents commentContents, ILogger<CommentController> logger)
    {
        _logger = logger;
        _commentContents = commentContents;
    }
    
    [HttpGet("get/{topicId}")]
    public IActionResult GetComments(string topicId)
    {
        try
        {
            List<Comment> comments = _commentContents.GetAllComments(topicId);
            return Ok(comments);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting comments");
        }
        return BadRequest("Error getting comments");
    }
    [HttpGet("getComment/{commentId}")]
    public IActionResult GetCommentWithID(string commentId)
    {
        try
        {
            Comment? comment = _commentContents.GetCommentById(commentId);
            return Ok(comment);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting comments");
        }
        return BadRequest("Error getting comments");
    }
    
    [HttpPost("add")]
    public IActionResult AddComment([FromBody] JsonElement json)
    {
        string? message = json.GetProperty("message").GetString();
        string? topicId = json.GetProperty("topicId").GetString();
        string? userId = json.GetProperty("userId").GetString();
        
        if (message == null || topicId == null || userId == null)
        {
            return BadRequest("Invalid request body");
        }

        Comment comment = new Comment(userId, topicId, message);
        _logger.LogInformation("Comment message: {comment}", json);
        try
        {
            bool result = _commentContents.AddComment(comment);
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
    
    [HttpDelete("delete/{commentId}")]
    public IActionResult DeleteComment(string commentId)
    {
        try
        {
            bool result = _commentContents.Remove(commentId);
            if (result)
            {
                return Ok("Comment deleted successfully");
            }
            return BadRequest("Failed to delete comment");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting comment");
            return BadRequest("Error deleting comment");
        }
    }

}