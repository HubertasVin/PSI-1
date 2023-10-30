namespace Project.Models;

public class TopicModel : BaseModel
{
    public string Name { get; set; }
    public Subject Subject { get; init; }

    public TopicModel()
    {
        
    }

    public TopicModel(string name, Subject subject)
    {
        Name = name;
        Subject = subject;
    }
}