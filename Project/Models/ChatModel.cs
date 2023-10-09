using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Project.Hubs;
namespace Project.Models
{
    public class ChatModel : ChatHub
    {
        // Task record struct
        public record struct MessageData : IComparable<MessageData> // struct used to store message data
        {
            // Task Properties 1
            public string User { get; set; }
            public string Message { get; set; }
            public DateTime Timestamp { get; set; }

            public int CompareTo(MessageData other) // CompareTo method used to sort messages by timestamp in descending order
            {
                return other.Timestamp.CompareTo(this.Timestamp);
            }
        }

        public ChatModel() {}
    }
}
