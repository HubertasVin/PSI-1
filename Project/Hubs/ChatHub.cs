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
        
        public async Task DeleteMessage(string topicId, string messageId)
        {
            Console.WriteLine("Deleting message: " + messageId + "in topic:" + topicId);
            await Clients.Group(topicId).SendAsync("DeleteMessage", messageId);
        }
        
        public async Task JoinTopic(string topicId)
        {
            Console.WriteLine("Joining topic: " + topicId);
            await Groups.AddToGroupAsync(Context.ConnectionId, topicId);
        }
        
        public async Task LeaveTopic(string topicId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, topicId);
        }
    }
}
