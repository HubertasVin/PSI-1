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

    public SubjectController()
    {
        if(_subjectContents == null)
            _subjectContents = new SubjectContents();
        _subjectContents.InitContents();
    }
    // public SubjectController(SubjectContents subjectContents)
    // {
    //     _subjectContents = subjectContents;
    // }

    [HttpGet("list")]
    public IActionResult ListSubjects()
    {
        return Ok(_subjectContents.GetSubjectsList());
    }
    
    [HttpPost("upload")]
    public IActionResult UploadSubject([FromBody] JsonElement request)
    {
        Console.WriteLine("In upload subject");
        Console.WriteLine(request);
        Subject? addedSubject = _subjectContents.CreateSubject(request);
        return addedSubject == null
            ? BadRequest("Invalid request body")
            : Ok(addedSubject);
    }
}