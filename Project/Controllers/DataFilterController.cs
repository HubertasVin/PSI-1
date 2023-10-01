using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Project.Controllers
{
    public class DataFilterController : Controller
    {
        public class Concept
        {
            public required string Name { get; set; }
            public required DateTime Date { get; set; }
            public required int Size { get; set; }
            public required string User { get; set; }
        }

        // Define a method to filter concepts by user
        [HttpGet("FilterByUser")]
        public IActionResult FilterByUser(string userName)
        {
            try
            {
                // Get the application's root directory
                string rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                // Construct the full path to the "data.json" file in the "src" folder
                string jsonFilePath = Path.Combine(rootPath, "src", "SignalIRData.json");

                // Read the JSON data from the file into a string
                string json = System.IO.File.ReadAllText(jsonFilePath);

                // Deserialize the JSON into a list of Concept objects
                List<Concept> concepts = JsonConvert.DeserializeObject<List<Concept>>(json);

                // Use LINQ to filter concepts by user
                var filteredConcepts = concepts.Where(c => c.User == userName).ToList();

                return Ok(filteredConcepts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // Define other methods to filter concepts based on different criteria (e.g., by date, size, etc.)
        // Add additional actions as needed to handle various filtering requirements.
    }
}
