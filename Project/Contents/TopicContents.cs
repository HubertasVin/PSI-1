using System.Text.Json;
using Project.Chat;
using Project.Data;
using Project.Models;

namespace Project.Contents;

public class TopicContents : Contents<Topic>
{
    public NoteBlendDbContext NoteBlendContext => Context as NoteBlendDbContext;
    // private List<Topic> _topicList;
    // private string _filepath = "src/TopicData.json";
    // private SubjectContents _subjectContents = new SubjectContents();
    
    public TopicContents(NoteBlendDbContext context) : base(context)
    {
        
    }
    
    public List<Topic> GetTopicsList(string subjectId)
    {
        return NoteBlendContext.Topics.Select(topic => topic).Where(topic => topic.Subject.id == subjectId).ToList();
    }

    public Topic? CreateTopic(JsonElement req)
    {
        if (!req.TryGetProperty("topicName", out var topicNameProperty) ||
            !req.TryGetProperty("subjectId", out var subjectIdProperty))
            return null;
        
        string? topicName = topicNameProperty.GetString();
        string? subjectId = subjectIdProperty.GetString();
        
        Console.WriteLine("Topic name: " + topicName);
        Console.WriteLine("Subject ID: " + subjectId);
        
        Subject? subject = NoteBlendContext.Subjects.Find(subjectId);
        
        Console.WriteLine($"Creating new topic named: {topicName}");
        Console.WriteLine($"Subject ID: {subjectId}");
        Topic newTopic = new Topic(topicName, subject);
        Add(newTopic);
        int changes = NoteBlendContext.SaveChanges();
        // JSONParser.WriteToJSON(_filepath, _topicList);
        return changes > 0 ? newTopic : null;
    }
}