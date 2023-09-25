using Microsoft.AspNetCore.SignalR;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Project.InOut;
using static Project.Models.InOutModel;

namespace Project.Hubs
{
    public class ChatHub : Hub
    {
        protected string path = "src/SignalRData.json";
        protected readonly List<MessageData> chatMessages = new List<MessageData>();
        public async Task SendMessage(string user, string message)
        {
            var chatMessage = new MessageData
            {
                User = user,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            chatMessages.Add(chatMessage);
            JSONParser.AddJSONMessage(chatMessage.User, chatMessage.Message, chatMessage.Timestamp, path, chatMessages);
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async void GetAllMessages() 
        {
            foreach (MessageData msg in chatMessages)
            {
                await Clients.All.SendAsync("LoadMessages", msg.User, msg.Message);
            }
        }
    }
}
