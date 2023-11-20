using Microsoft.AspNetCore.SignalR;

namespace Project.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string topicId, string userId, string message)
        {
            Console.WriteLine("Sending to: " + topicId + " " + userId + " " + message);
            await Clients.Group(topicId).SendAsync("ReceiveMessage", topicId, userId, message);
        }
        
        public async Task JoinTopic(string topicId)
        {
            Console.WriteLine("Joining topic: " + topicId);
            await Groups.AddToGroupAsync(Context.ConnectionId, topicId);
            
            // await Clients.Group(topicId).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined the group {topicId}.");
        }
        
        public async Task LeaveTopic(string topicId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, topicId);
        }
        // public async Task DeleteMessage(string messageId)
        // {
        //     await Clients.Group(topicId).
        // }
        // protected string path = "src/SignalRData.json"; // Specifying the path for debugging purposes
        // protected List<MessageData> chatMessages = new List<MessageData>(); // Possible generic type for task 7
        //
        // // Task optional argument
        // public async Task SendMessage(string user, string message) // Sends a message to all clients including the sender (chat.js calls this)
        // {
        //     var chatMessage = new MessageData
        //     {
        //         User = user,
        //         Message = message,
        //         Timestamp = DateTime.UtcNow,
        //     };
        //
        //     chatMessages.Add(chatMessage);
        //     // Task named arguments
        //     JSONParser.AddJSONMessage(path: path, messageData: chatMessage, messages: chatMessages);
        //     await Clients.All.SendAsync("ReceiveMessage", chatMessage.User, chatMessage.Message, chatMessage.Timestamp.ToString()); // Sends the message to all clients
        // }
        //
        // public void OptionalArgSend() // Proper usage of optional args
        // {
        //     JSONParser.AddJSONMessageOptional(chatMessages, message: "testas123");
        // }
        //
        // public async Task LoadMessage() // Loads all messages from the JSON file and sends them to the client (chat.js calls this)
        // {
        //     chatMessages = JSONParser.ReadFromJSON<MessageData>(path);
        //     List<Task> listOfTasks = new List<Task>(); // List of tasks to be completed
        //     foreach (MessageData msg in chatMessages)
        //     {
        //         listOfTasks.Add(Clients.Caller.SendAsync("ReceiveMessage", msg.User, msg.Message, msg.Timestamp.ToString())); // Adds a task to the list
        //     }
        //     await Task.WhenAll(listOfTasks); // Waits for all tasks to be completed
        // }
    }
}
