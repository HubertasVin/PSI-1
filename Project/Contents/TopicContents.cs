using System.Text.Json;
using Project.Chat;
using Project.Models;

namespace Project.Contents;

public class TopicContents
{
    private List<Topic> _topicList;
    private string _filepath = "src/TopicData.json";
    private SubjectContents _subjectContents = new SubjectContents();
    
    public TopicContents()
    {
        InitContents();
    }
    
    public void InitContents()
    {
        _topicList = JSONParser.ReadFromJSON<Topic>(_filepath);
        Console.WriteLine(_topicList);
    }
    
    public Topic GetTopic(string id)
    {
        return _topicList.Find(topic => topic.id == id);
    }
    
    public List<Topic> GetTopicsList(string subjectId)
    {
        return _topicList.Select(topic => topic).Where(topic => topic.Subject.id == subjectId).ToList();
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
        
        Subject? subject = _subjectContents.GetSubject(subjectId);
        
        Console.WriteLine($"Creating new topic named: {topicName}");
        Console.WriteLine($"Subject ID: {subjectId}");
        Topic newTopic = new Topic(topicName, subject);
        _topicList.Add(newTopic);
        JSONParser.WriteToJSON(_filepath, _topicList);
        return newTopic;
    }
}