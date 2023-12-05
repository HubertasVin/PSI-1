using Project.Data;
using Project.Models;

namespace Project.Repository;

public class CommentRepository : Repository<Comment>, ICommentRepository
{
    public NoteBlendDbContext NoteBlendContext => Context as NoteBlendDbContext;
    
    public CommentRepository(NoteBlendDbContext context) : base (context)
    {
    }
    
    public virtual Comment? GetCommentById(string commentId)
    {
        return NoteBlendContext.Comments.Find(commentId);
    }
    public virtual List<Comment> GetAllComments(string topicId)
    {
        return Find(comment => comment.TopicId.Equals(topicId)).ToList();
    }

    public virtual bool Remove(string commentId)
    {
        Comment comment = Get(commentId);
        Remove(comment);
        int changes = NoteBlendContext.SaveChanges();
        return changes > 0;
    }
    
    public virtual Comment AddComment(Comment comment)
    {
        Add(comment);
        int changes = NoteBlendContext.SaveChanges();
        return changes > 0 ? comment : null;
    }

    public virtual int saveChanges()
    {
        return NoteBlendContext.SaveChanges();
    }
}