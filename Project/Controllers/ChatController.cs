using Microsoft.AspNetCore.Mvc;
using Project.Models;
using System.Diagnostics;

namespace JsonParsing;

public class ChatController : Controller
{
    // returns a localhost/Chat view
    public IActionResult Index() 
    {
        return View();
    }

    public IActionResult ShowChatSearch() // returns Chat/ShowChatSearch
    {
        return View();
    }
    
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
