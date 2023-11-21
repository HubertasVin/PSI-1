using Microsoft.AspNetCore.SignalR;
using Project.Models;
using Project.Services;

namespace Project.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;

        public ChatHub(ChatService chatService)
        {
            _chatService = chatService;
        }
        
        public async Task SendMessage(string topicId, string userId, string message)
        {
            Comment? newComment = _chatService.SaveCommentToDb(userId, topicId, message);
            if (newComment != null)
            {
                await Clients.Group(topicId).SendAsync("ReceiveMessage", newComment.id, topicId, userId, message);
            }
            Console.WriteLine("Sending to: " + topicId + " " + userId + " " + message);
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
        
        public async Task DeleteMessage(string messageId, string topicId)
        {
            await Clients.Group(topicId).SendAsync("DeleteMessage", messageId);
        }
    }
}
