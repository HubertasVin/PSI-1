using Project.Models;
using Project.Repository;

namespace Project.Services;

public class ChatService
{
    private readonly ICommentRepository _commentRepository;
    private readonly ILogger<ChatService> _logger;

    public ChatService(ICommentRepository commentRepository, ILogger<ChatService> logger)
    {
        _logger = logger;
        _commentRepository = commentRepository;
    }

    public Comment? SaveCommentToDb(string userId, string topicId, string message)
    {
        // TODO: ADD check for user if exists
        
        Comment newComment = new Comment(userId, topicId, message);
        _commentRepository.AddComment(newComment);
        return newComment;
    }
}