using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using static Project.Models.ChatModel;


namespace Project.Controllers
{
    public class DataFilterController : Controller
    {
        private readonly List<MessageData> chatMessages;

        public DataFilterController()
        {
            // Load data from the JSON file into the list when the controller is initialized.
            var json = System.IO.File.ReadAllText("src/SignalRData.json");
            chatMessages = JsonConvert.DeserializeObject<List<MessageData>>(json);
        }

        [HttpGet]
        [Route("DataFilter/GetFilteredData")]
        public IActionResult GetFilteredData(string searchMessage = null, string searchUsername = null, DateTime? searchDate = null)
        {
            var filteredMessages = chatMessages;

            // Filter by message content
            if (!string.IsNullOrEmpty(searchMessage))
            {
                filteredMessages = filteredMessages.Where(m => m.Message.Contains(searchMessage)).ToList();
            }

            // Filter by username
            if (!string.IsNullOrEmpty(searchUsername))
            {
                filteredMessages = filteredMessages.Where(m => m.User == searchUsername).ToList();
            }

            // Filter by date
            if (searchDate.HasValue)
            {
                filteredMessages = filteredMessages.Where(m => m.Timestamp.Date == searchDate.Value.Date).ToList();
            }

            // Return the view with filtered messages
            return View("FilteredMessages", filteredMessages);
        }

    }
}
