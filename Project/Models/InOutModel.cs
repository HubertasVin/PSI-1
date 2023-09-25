using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Project.Hubs;
using Project.InOut;

namespace Project.Models
{
    public class InOutModel : ChatHub
    {
        public struct MessageData 
        {
            public string User { get; set; }
            public string Message { get; set; }
            public DateTime Timestamp { get; set; }
        }

        // public List<InOut.JSONParser.MessageData> Messages { get; set; }
        // public string InputMessage { get; set; }

        public InOutModel()
        {
            var chatHistory = new JSONParser(path, chatMessages);
            // this.Messages = JSONParser.Messages;
            // this.InputMessage = string.Empty; // Initialize InputMessage in the constructor
            // file.Close(); // Won't work on windows without this 
        }

        public void AddMessage(string user, string message, DateTime date, string path, List<MessageData> Messages)
        {
            Project.InOut.JSONParser.AddJSONMessage(user, message, date, path, Messages);
        }
    }
}
