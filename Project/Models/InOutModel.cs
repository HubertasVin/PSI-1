using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Project.Hubs;
using static Project.InOut.JSONParser;

namespace Project.Models
{
    public class InOutModel : ChatHub
    {
        public struct MessageData // struct used to store message data
        {
            public string User { get; set; }
            public string Message { get; set; }
            public DateTime Timestamp { get; set; }
        }

        public InOutModel() {}
    }
}
