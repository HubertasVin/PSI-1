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

        public InOutModel()
        {
            var chatHistory = new JSONParser(path, chatMessages);
        }

        public void AddMessage(string user, string message, DateTime date, string path, List<MessageData> Messages)
        {
            Project.InOut.JSONParser.AddJSONMessage(user, message, date, path, Messages);
        }
    }
}
