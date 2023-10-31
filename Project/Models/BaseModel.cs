using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public class BaseModel
{
    [Key]
    public string id { get; init; } = Guid.NewGuid().ToString();
}