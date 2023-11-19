namespace Project.Models;

public class Comment : BaseModel
{
    public string Message { get; set; }
    public string UserId { get; set; }
    public string TopicId { get; set; }
    
    public Comment(string userId, string topicId, string message)
    {
        UserId = userId;
        TopicId = topicId;
        Message = message;
    }
}