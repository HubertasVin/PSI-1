using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Project.Exceptions;
using Project.Models;
using Project.Repository;

namespace Project.Controllers;

[ApiController]
[Route("[controller]")]
public class TopicController : ControllerBase
{
    private readonly ITopicRepository _topicRepository;
    private readonly ILogger<TopicController> _logger;

    public TopicController(ITopicRepository topicRepository, ILogger<TopicController> logger)
    {
        _topicRepository = topicRepository;
        _logger = logger;
    }
    
    [HttpGet("get/{id}")]
    public IActionResult GetTopic(string id)
    {
        try
        {
            return Ok(_topicRepository.GetTopic(id));
        }
        catch (ObjectNotFoundException)
        {
            _logger.LogWarning("Topic with id {id} could not be found", id);
            return NotFound("Topic not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting topic with id {id}", id);
            return BadRequest("Topic not found");
        }
        
    }
    
    [HttpGet("list/{subjectId}")]
    public IActionResult ListTopics(string subjectId)
    {
        try
        {
            return Ok(_topicRepository.GetTopicsList(subjectId));
        }
        catch (ObjectNotFoundException)
        {
            _logger.LogWarning("Subject with id {id} could not be found", subjectId);
            return NotFound("Subject not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting topics for subject with id {id}", subjectId);
            return BadRequest("Subject not found");
        }
        
    }

    [HttpPost("upload")]
    public IActionResult UploadTopic([FromBody] JsonElement request)
    {
        Topic? addedTopic = _topicRepository.CreateTopic(request);
        return addedTopic == null
            ? BadRequest("Invalid request body")
            : Ok(addedTopic);
    }
}