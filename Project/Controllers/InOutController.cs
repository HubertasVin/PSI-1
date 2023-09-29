using Microsoft.AspNetCore.Mvc;
using Project.Models;
using System.Diagnostics;

namespace JsonParsing;

public class InOutController : Controller
{
    // returns a localhost/InOut view
    public IActionResult Index() 
    {
        return View(); //passing model so that I could see content on web page
    }
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
