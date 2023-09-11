using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Project.Models;

namespace Project.Controllers;

public class InOutController : Controller
{
    // returns a localhost/InOut view
    public IActionResult Index() 
    {
        var InOutModel = new InOutModel { Message = "Testing123" };
        return View(InOutModel); //passing model so that I could see content on web page
    }
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}