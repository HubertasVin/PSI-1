using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Web;
using Project.Exceptions;
using Project.Models;
using Project.Repository;
 
namespace Project.Controllers;
 
[ApiController]
[Route("[controller]")]
public class ConspectController : ControllerBase
{
    private readonly IConspectRepository _conspectRepository;
    private readonly ILogger<ConspectController> _logger;
 
    public ConspectController(IConspectRepository conspectRepository, ILogger<ConspectController> logger)
    {
        _conspectRepository = conspectRepository;
        _logger = logger;
    }
 
    [HttpGet("get/{id}/{index}")]
    public IActionResult GetConspect(string id, int index)
    {
        try {
            return Ok(_conspectRepository.GetConspectByTopicIdAndIndex(id, index));
        }
        catch (ConspectNotFoundException e) {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpGet("get-conspect-file/{id}/{index}")]
    public IActionResult GetConspectFile(string id, int index)
    {
        try {
            Conspect? conspect = _conspectRepository.GetConspectByTopicIdAndIndex(id, index) ?? throw new ConspectNotFoundException("Conspect not found");
            return File(System.IO.File.ReadAllBytes(conspect.ConspectLocation), "application/pdf");
        }
        catch (ConspectNotFoundException e) {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }
 
    [HttpGet("get-conspects-list-by-id/{id}")]
    public IActionResult GetUserByEmail(string id)
    {
        try {
            return Ok(_conspectRepository.GetConspectListByTopicId(id));
        }
        catch (ConspectNotFoundException e) {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }
 
    [HttpGet("list")]
    public IActionResult ListConspects()
    {
        return Ok(_conspectRepository.GetConspectList());
    }
 
    [HttpPost("upload")]
    public async Task<IActionResult> UploadConspect([FromForm] Conspect newConspect, [FromForm] IFormFile file)
    {
        try {
            return Ok(await _conspectRepository.CreateConspect(newConspect, file));
        }
        catch (ConspectAlreadyExistsException e) {
            _logger.LogError("Error occured when uploading a new conspect", e);
            return BadRequest(e.Message);
        }
    }
}