using Microsoft.AspNetCore.SignalR;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using static Project.InOut.JSONParser;
using static Project.Models.InOutModel;

namespace Project.Hubs
{
    public class ChatHub : Hub
    {
        protected string path = "src/SignalRData.json";
        protected List<MessageData> chatMessages = new List<MessageData>();
        
        public async Task SendMessage(string user, string message)
        {
            var chatMessage = new MessageData
            {
                User = user,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            chatMessages.Add(chatMessage);
            AddJSONMessage(chatMessage.User, chatMessage.Message, chatMessage.Timestamp, path, chatMessages);
            await Clients.All.SendAsync("ReceiveMessage", chatMessage.User, chatMessage.Message, chatMessage.Timestamp.ToString());
        }

        public async Task LoadMessage() 
        {
            chatMessages = ReadMessagesFromJSON(path);
            List<Task> listOfTasks = new List<Task>();
            Console.WriteLine("LoadMessage called");
            Console.WriteLine(chatMessages.Count);
            foreach (MessageData msg in chatMessages)
            {
                Console.WriteLine("Contents: user:" + msg.User + " | " + msg.Message + " | " + msg.Timestamp);
                listOfTasks.Add(Clients.Caller.SendAsync("ReceiveMessage", msg.User, msg.Message, msg.Timestamp.ToString()));
            }
            await Task.WhenAll(listOfTasks);
        }

    }
}
