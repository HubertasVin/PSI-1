namespace Project.Models;

public class BaseModel
{
    public string id { get; init; } = Guid.NewGuid().ToString();
}