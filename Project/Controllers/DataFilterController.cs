using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Packaging.Signing;
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
    public class DataFilterController : Controller, IComparable<MessageData>
    {
        private readonly List<MessageData> chatMessages;

        public enum SortType
        {
            Ascending,
            Descending
        }

        public DataFilterController()
        {
            // Load data from the JSON file into the list when the controller is initialized.
            var json = System.IO.File.ReadAllText("src/SignalRData.json");
            chatMessages = JsonConvert.DeserializeObject<List<MessageData>>(json);
        }

        [HttpGet]
        [Route("DataFilter/GetFilteredData")]
        public IActionResult GetFilteredData(
            string searchMessage = null,
            string searchUsername = null,
            DateTime? searchDate = null,
            SortType sortType = SortType.Ascending
        )
        {
            var filteredMessages = chatMessages;
            Console.WriteLine(sortType);
            if (sortType == SortType.Ascending)
                Console.WriteLine("Woohoo sortBy is ascending");
            else
                Console.WriteLine("sortBy is descending");

            // Filter by message content
            if (!string.IsNullOrEmpty(searchMessage))
            {
                filteredMessages = filteredMessages
                    .Where(m => m.Message.Contains(searchMessage))
                    .ToList();
            }

            // Filter by username
            if (!string.IsNullOrEmpty(searchUsername))
            {
                filteredMessages = filteredMessages.Where(m => m.User == searchUsername).ToList();
            }

            // Filter by date
            if (searchDate.HasValue)
            {
                filteredMessages = filteredMessages
                    .Where(m => m.Timestamp.Date == searchDate.Value.Date)
                    .ToList();
            }

            // Return the view with filtered messages
            return View("FilteredMessages", filteredMessages);
        }

        public int CompareTo(MessageData other)
        {
            throw new NotImplementedException();
        }
    }
}
