namespace Project.Models;

public class Note : BaseModel
{
    public string Name { get; set; }
    // public List<Message> Messages { get; set; }
    
    public Note()
    {
        
    }
    
    public Note(string name)
    {
        Name = name;
    }
    
    // public void AddMessage(Message message)
    // {
    //     Messages.Add(message);
    // }
}