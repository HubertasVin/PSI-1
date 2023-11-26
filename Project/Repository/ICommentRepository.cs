using Project.Models;

namespace Project.Repository;

public interface ICommentRepository
{
    public Comment? GetCommentById(string commentId);
    public List<Comment> GetAllComments(string topicId);
    public bool Remove(string commentId);
    public Comment AddComment(Comment comment);
}