namespace Project.Models;

public class InOutModel
{
    // public int Id { get; set; }
    public string Message { get; set; }
    
    public InOutModel(string path)
    {
        Message = File.ReadAllText(path);
    }
}