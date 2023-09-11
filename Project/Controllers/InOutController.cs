using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Project.Models;

namespace Project.Controllers;

public class InOutController : Controller
{
    private readonly ILogger<InOutController> _logger;
    
    public InOutController(ILogger<InOutController> logger)
    {
        _logger = logger;
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}