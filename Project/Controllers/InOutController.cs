using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
// using Renci.SshNet;
using Xunit;
using Xunit.Abstractions;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Project.Models;
using System.Diagnostics;

namespace JsonParsing;

public class InOutController : Controller
{
    [HttpPost]
    public IActionResult AddMessage(string InputMessage)
    {
        FileStream file = System.IO.File.OpenRead("src/data.json");
        var InOutModel = new InOutModel(file);
        InOutModel.AddMessage("Guest", InputMessage, "src/data.json");
        return RedirectToAction("Index");
    }
    
    // returns a localhost/InOut view
    public IActionResult Index() 
    {
        using FileStream file = System.IO.File.OpenRead("src/data.json");
        var InOutModel = new InOutModel(file);
        return View(InOutModel); //passing model so that I could see content on web page
    }
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
