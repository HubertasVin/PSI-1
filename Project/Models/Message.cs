namespace Project.Models;

public class Message : BaseModel
{
    public string Text { get; set; }
    public DateTime Date { get; set; }
    
    public Message(string text, DateTime date)
    {
        Text = text;
        Date = date;
    }
}