namespace Project.Models;

public class Note : BaseModel
{
    public string Name { get; set; }
    
    public Note(string name)
    {
        Name = name;
    }
}