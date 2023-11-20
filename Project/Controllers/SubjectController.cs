using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Repository;

namespace Project.Controllers;

[ApiController]
[Route("[controller]")]
public class SubjectController : ControllerBase
{
    private readonly SubjectRepository _subjectRepository;

    public SubjectController(SubjectRepository subjectRepository)
    {
        _subjectRepository = subjectRepository;
    }

    [HttpGet("get/{id}")]
    public IActionResult GetSubject(string id)
    {
        Subject? subject = _subjectRepository.GetSubject(id);
        return subject == null
            ? NotFound(new { error = $"Subject with id {id} could not be found" })
            : Ok(subject);
    }
    
    [HttpGet("list")]
    public IActionResult ListSubjects()
    {
        return Ok(_subjectRepository.GetSubjectsList());
    }
    
    [HttpPost("upload")]
    public IActionResult UploadSubject([FromBody] Subject newSubject)
    {
        Subject? addedSubject = _subjectRepository.CreateSubject(newSubject);
        return addedSubject == null
            ? BadRequest("Invalid request body")
            : Ok(addedSubject);
    }
}