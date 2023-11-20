using Project.Data;
using Project.Models;

namespace Project.Repository;

public class CommentRepository : Repository<Comment>
{
    public NoteBlendDbContext NoteBlendContext => Context as NoteBlendDbContext;
    
    public CommentRepository(NoteBlendDbContext context) : base (context)
    {
    }
    
    public Comment? GetCommentById(string commentId)
    {
        return NoteBlendContext.Comments.Find(commentId);
    }
    public List<Comment> GetAllComments(string topicId)
    {
        return NoteBlendContext.Comments
            .Select(comment => comment)
            .Where(comment => comment.TopicId.Equals(topicId)).ToList();
    }

    public bool Remove(string CommentId)
    {
        Comment comment = Get(CommentId);
        Remove(comment);
        int changes = NoteBlendContext.SaveChanges();
        return changes > 0;
    }
    
    public bool AddComment(Comment comment)
    {
        Add(comment);
        int changes = NoteBlendContext.SaveChanges();
        return changes > 0;
    }
}