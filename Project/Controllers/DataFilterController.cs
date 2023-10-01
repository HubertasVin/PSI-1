using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using static Project.Models.InOutModel;

namespace Project.Controllers
{
    public class DataFilterController : ControllerBase
    {
        private readonly List<MessageData> chatMessages;

        public DataFilterController()
        {
            // Load data from the JSON file into the list when the controller is initialized.
            var json = System.IO.File.ReadAllText("src/SignalRData.json");
            chatMessages = JsonConvert.DeserializeObject<List<MessageData>>(json);
        }

        [HttpGet]
        public IActionResult GetFilteredData(string username = null, string messageContains = null, DateTime? timestamp = null)
        {
            var filteredMessages = chatMessages;

            // Filter by username
            if (!string.IsNullOrEmpty(username))
            {
                filteredMessages = filteredMessages.Where(m => m.User == username).ToList();
            }

            // Filter by message containing a word
            if (!string.IsNullOrEmpty(messageContains))
            {
                filteredMessages = filteredMessages.Where(m => m.Message.Contains(messageContains)).ToList();
            }

            // Filter by timestamp (if provided)
            if (timestamp.HasValue)
            {
                filteredMessages = filteredMessages.Where(m => m.Timestamp == timestamp.Value).ToList();
            }

            return Ok(filteredMessages);
        }
    }
}