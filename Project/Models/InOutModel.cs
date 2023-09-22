using System.Text.Json;

namespace Project.Models;

public class InOutModel
{
    public struct MessageData
    {
        public string Name { get; set; }
        public string Text { get; set; }
    }
    // public int Id { get; set; }
    public MessageData[] Message { get; set; }
    
    public InOutModel(FileStream file)
    {
        JsonElement json = JsonDocument.Parse(file).RootElement;
        if (json.ValueKind != JsonValueKind.Array) return;

        List<MessageData> messages = new List<MessageData>();

        foreach (JsonElement element in json.EnumerateArray())
        {
            if (element.TryGetProperty("name", out JsonElement name))
            {
                MessageData message = new MessageData();
                message.Name = name.GetString();
                if (element.TryGetProperty("message", out JsonElement text))
                    message.Text = text.GetString();
                messages.Add(message);
                System.Console.WriteLine(message.Name?.ToString());
            }
        }

        Message = messages.ToArray();
    }
}