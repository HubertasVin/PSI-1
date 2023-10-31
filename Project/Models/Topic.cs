namespace Project.Models;

public class Topic : BaseModel
{
    public string Name { get; set; }
    public Subject Subject { get; init; }

    public Topic()
    {
        
    }

    public Topic(string name, Subject subject)
    {
        Name = name;
        Subject = subject;
    }
}