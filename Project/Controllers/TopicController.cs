using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Repository;

namespace Project.Controllers;

[ApiController]
[Route("[controller]")]
public class TopicController : ControllerBase
{
    private readonly TopicRepository _topicRepository;

    public TopicController(TopicRepository topicRepository)
    {
        _topicRepository = topicRepository;
    }
    
    [HttpGet("get/{id}")]
    public IActionResult GetTopic(string id)
    {
        return Ok(_topicRepository.Get(id));
    }
    
    [HttpGet("list/{subjectId}")]
    public IActionResult ListTopics(string subjectId)
    {
        return Ok(_topicRepository.GetTopicsList(subjectId));
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