using Microsoft.AspNetCore.SignalR;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using static Project.InOut.JSONParser;
using static Project.Models.InOutModel;

namespace Project.Hubs
{
    public class ChatHub : Hub
    {
        protected string path = "src/SignalRData.json"; // Specifying the path for debugging purposes
        protected List<MessageData> chatMessages = new List<MessageData>();
        
        public async Task SendMessage(string user, string message) // Sends a message to all clients including the sender (chat.js calls this)
        {
            var chatMessage = new MessageData
            {
                User = user,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            chatMessages.Add(chatMessage);
            AddJSONMessage(chatMessage, path, chatMessages);
            await Clients.All.SendAsync("ReceiveMessage", chatMessage.User, chatMessage.Message, chatMessage.Timestamp.ToString()); // Sends the message to all clients
        }

        public async Task LoadMessage() // Loads all messages from the JSON file and sends them to the client (chat.js calls this)
        {
            chatMessages = ReadFromJSON<MessageData>(path);
            List<Task> listOfTasks = new List<Task>(); // List of tasks to be completed
            Console.WriteLine("LoadMessage called"); // FOR DEBUGGING PURPOSES
            Console.WriteLine(chatMessages.Count); // FOR DEBUGGING PURPOSES
            foreach (MessageData msg in chatMessages)
            {
                Console.WriteLine("Contents: user:" + msg.User + " | " + msg.Message + " | " + msg.Timestamp); // FOR DEBUGGING PURPOSES
                listOfTasks.Add(Clients.Caller.SendAsync("ReceiveMessage", msg.User, msg.Message, msg.Timestamp.ToString())); // Adds a task to the list
            }
            await Task.WhenAll(listOfTasks); // Waits for all tasks to be completed
        }

    }
}
