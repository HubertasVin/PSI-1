namespace Project.Models;

public class Subject : BaseModel
{
    public string Name { get; init; }

    public Subject(string name)
    {
        Name = name;
    }
}