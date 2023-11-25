using System.Text.Json;
using Project.Models;

namespace Project.Repository;

public interface ITopicRepository
{
    public Topic? GetTopic(string id);
    public List<Topic> GetTopicsList(string subjectId);
    public Topic? CreateTopic(JsonElement req);
}