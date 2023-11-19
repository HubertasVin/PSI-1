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
    public IActionResult GetCommendWithID(string commentId)
    {
        try
        {
            Comment comment = _commentContents.GetCommentById(commentId);
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
            bool result = _commentContents.AddComment(comment);
            if (result)
            {
                return Ok("Comment added successfully");
            }
            else
            {
                return BadRequest("Failed to add comment");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error adding comment");
            return BadRequest("Error adding comment");
        }
    }

}