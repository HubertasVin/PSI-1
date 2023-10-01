using Project.Hubs;
using static Project.InOut.JSONParser;

namespace Project.Models
{
    public class InOutModel : ChatHub
    {
        // Task enum
        public enum MessagePriority
        {
            Low,
            Normal,
            High
        }
        // Task record struct
        public record struct MessageData // struct used to store message data
        {
            // Task Properties 1
            public string User { get; set; }
            public string Message { get; set; }
            public DateTime Timestamp { get; set; }
            public MessagePriority Priority { get; set; }
        }

        public InOutModel() {}
    }
}
