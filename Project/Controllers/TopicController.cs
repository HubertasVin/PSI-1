using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Project.Contents;
using Project.Models;

namespace Project.Controllers;

[ApiController]
[Route("[controller]")]
public class TopicController : ControllerBase
{
    private readonly TopicContents _topicContents;

    public TopicController(TopicContents topicContents)
    {
        _topicContents = topicContents;
    }
    
    [HttpGet("get/{id}")]
    public IActionResult GetTopic(string id)
    {
        return Ok(_topicContents.Get(id));
    }
    
    [HttpGet("list/{subjectId}")]
    public IActionResult ListTopics(string subjectId)
    {
        return Ok(_topicContents.GetTopicsList(subjectId));
    }

    [HttpPost("upload")]
    public IActionResult UploadTopic([FromBody] JsonElement request)
    {
        Console.WriteLine("In upload topic");
        // Console.WriteLine(request);
        Topic? addedTopic = _topicContents.CreateTopic(request);
        return addedTopic == null
            ? BadRequest("Invalid request body")
            : Ok(addedTopic);
    }
}