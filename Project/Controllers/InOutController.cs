using Microsoft.AspNetCore.Mvc;
using Project.Models;
using System.Diagnostics;

namespace JsonParsing;

public class InOutController : Controller
{
    // public IActionResult AddMessage(string InputMessage)
    // {
    //     FileStream file = System.IO.File.OpenRead("src/data.json");
    //     var InOutModel = new InOutModel(file);
    //     InOutModel.AddMessage("Guest", InputMessage, "src/data.json");
    //     return RedirectToAction("Index");
    // }
    
    // returns a localhost/InOut view
    public IActionResult Index() 
    {
        // using FileStream file = System.IO.File.OpenRead("src/SignalRData.json");
        var InOutModel = new InOutModel();
        return View(); //passing model so that I could see content on web page
    }
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
