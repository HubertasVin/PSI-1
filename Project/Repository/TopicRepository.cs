using System.Text.Json;
using Project.Data;
using Project.Models;

namespace Project.Repository;

public class TopicRepository : Repository<Topic>, ITopicRepository
{
    public NoteBlendDbContext NoteBlendContext => Context as NoteBlendDbContext;
    
    public TopicRepository(NoteBlendDbContext context) : base(context)
    {
        
    }
    
    public Topic? GetTopic(string id)
    {
        return Find(topic => topic.id == id).FirstOrDefault();
    }
    
    public List<Topic> GetTopicsList(string subjectId)
    {
        return Find(topic => topic.Subject.id == subjectId).ToList();
    }

    public Topic? CreateTopic(JsonElement req)
    {
        if (!req.TryGetProperty("topicName", out var topicNameProperty) ||
            !req.TryGetProperty("subjectId", out var subjectIdProperty))
            return null;
        
        string? topicName = topicNameProperty.GetString();
        string? subjectId = subjectIdProperty.GetString();
        
        Subject? subject = NoteBlendContext.Subjects.Find(subjectId);
        
        Console.WriteLine($"Creating new topic named: {topicName}");
        Console.WriteLine($"Subject ID: {subjectId}");
        Topic newTopic = new Topic(topicName, subject);
        Add(newTopic);
        int changes = NoteBlendContext.SaveChanges();
        return changes > 0 ? newTopic : null;
    }
}