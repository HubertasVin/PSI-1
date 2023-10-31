using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Project.Contents;
using Project.Models;

namespace Project.Controllers;

[ApiController]
[Route("[controller]")]
public class SubjectController : ControllerBase
{
    private readonly SubjectContents _subjectContents;

    public SubjectController(SubjectContents subjectContents)
    {
        _subjectContents = subjectContents;
    }
    // public SubjectController(SubjectContents subjectContents)
    // {
    //     _subjectContents = subjectContents;
    // }

    [HttpGet("get/{id}")]
    public IActionResult GetSubject(string id)
    {
        Subject? subject = _subjectContents.GetSubject(id);
        return subject == null
            ? NotFound(new { error = $"Subject with id {id} could not be found" })
            : Ok(subject);
    }
    
    [HttpGet("list")]
    public IActionResult ListSubjects()
    {
        return Ok(_subjectContents.GetSubjectsList());
    }
    
    [HttpPost("upload")]
    public IActionResult UploadSubject([FromBody] JsonElement request)
    {
        Console.WriteLine("In upload subject");
        // Console.WriteLine(request);
        Subject? addedSubject = _subjectContents.CreateSubject(request);
        return addedSubject == null
            ? BadRequest("Invalid request body")
            : Ok(addedSubject);
    }
}