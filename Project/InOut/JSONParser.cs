using System.Text.Json;
using Newtonsoft.Json;

namespace Project.InOut
{
    public class JSONParser
    {
        public struct MessageData
        {
            public string Name { get; set; }
            public string Text { get; set; }
        }

        public MessageData Message { get; set; } // Initialize Message with an empty array

        public static List<MessageData> Messages = new List<MessageData>();

        public JSONParser(FileStream file)
        {
            JsonElement json = JsonDocument.Parse(file).RootElement;
            if (json.ValueKind != JsonValueKind.Array || Messages.Count() != 0) return;

            foreach (JsonElement element in json.EnumerateArray())
            {
                if (element.TryGetProperty("Name", out JsonElement name))
                {
                    MessageData message = new MessageData();
                    message.Name = name.GetString() ?? string.Empty;
                    if (element.TryGetProperty("Text", out JsonElement text))
                        message.Text = text.GetString() ?? string.Empty;
                    Messages.Add(message);
                }
            }

            // Message = Messages.ToArray();
        }

        public static void AddJSONMessage(string name, string message, string path)
        {
            var jsonData = System.IO.File.ReadAllText(path);
            Messages.Clear();
            Messages = JsonConvert.DeserializeObject<List<MessageData>>(jsonData) ?? new List<MessageData>();

            Messages.Add(new MessageData()
            {
                Name = name,
                Text = message
            });

            jsonData = JsonConvert.SerializeObject(Messages);
            System.IO.File.WriteAllText(path, jsonData);
        }
    }
}